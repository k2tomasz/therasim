using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.Feedback
{
    public partial class FeedbackContainer : ComponentBase
    {
        [Inject] private IFeedbackService FeedbackService { get; set; } = null!;
        [Inject] private Kernel Kernel { get; set; } = null!;
        [Parameter] public Guid SessionId { get; set; }
        private IChatCompletionService _chatCompletionService = null!;
        private OpenAIPromptExecutionSettings _openAiPromptExecutionSettings = null!;
        private ChatHistory _messages = [];

        protected override async Task OnInitializedAsync()
        {
            _chatCompletionService = Kernel.GetRequiredService<IChatCompletionService>();
            _openAiPromptExecutionSettings = new();
            await LoadMessages();
        }

        private async Task LoadMessages()
        {
            var feedbacks = await FeedbackService.GetSessionFeedbacks(SessionId);
            if (feedbacks.Count == 0)
            {
                AddSystemMessage(GetSystemPrompt());
            }
            else
            {
                foreach (var feedbacksDto in feedbacks)
                {
                    AddUserMessage(feedbacksDto.Message);
                    AddAssistantMessage(feedbacksDto.Content);
                }
            }
        }

        private void AddSystemMessage(string message)
        {
            _messages.AddSystemMessage(message);
        }

        private void AddUserMessage(string message)
        {
            _messages.AddUserMessage(message);
        }

        private void AddAssistantMessage(string message)
        {
            _messages.AddAssistantMessage(message);
            StateHasChanged();
        }

        //public async Task GetFeedbackForAssistantMessage(string message)
        //{
        //    var sessionMessage = $"Client: {message}";
        //    await GetFeedback(sessionMessage);
        //}

        //public async Task GetFeedbackForUserMessage(string message)
        //{
        //    var sessionMessage = $"Student: {message}";
        //    await GetFeedback(sessionMessage);
        //}

        public async Task GetFeedback(string studentMessage, string clientMessage)
        {
            var sessionMessage = $"Student: {studentMessage}; Client: {clientMessage}";
            AddUserMessage(sessionMessage);
            var response = await _chatCompletionService.GetChatMessageContentsAsync(_messages, _openAiPromptExecutionSettings, Kernel);
            foreach (var chatMessageContent in response)
            {
                var newFeedback = chatMessageContent.Content;
                if (string.IsNullOrEmpty(newFeedback)) continue;
                AddAssistantMessage(newFeedback);
                await FeedbackService.AddSessionFeedback(SessionId, newFeedback, sessionMessage);
            }
        }

        private string GetSystemPrompt()
        {
            return @"Objective:
You are the Supervisor AI, an experienced psychotherapist responsible for overseeing and evaluating a simulated therapy session. For each pair of Student (unexperienced psychotherapist) and Client (simulated patient) responses, your task is twofold:
1. Provide constructive feedback on the Student's response.
2. Suggest the next best response the Student could give to the Client.

Your role is strictly to evaluate and guide the Student. You are not participating in the conversation but rather providing feedback and suggesting improved responses based on the conversation context.

Role Reference:
You should act like a highly experienced psychotherapist, similar to Carl Rogers or Irvin Yalom, known for their wisdom, empathy, and teaching ability. Your feedback should be constructive, insightful, and rooted in psychological principles. Your primary goal is to educate and guide the Student, fostering their development as a competent therapist.

Behavior and Tone:
- **Constructive:** Offer feedback that helps the Student learn and improve without discouraging them.
- **Insightful:** Explain the underlying psychological concepts behind effective or ineffective responses.
- **Guiding:** Suggest alternative approaches that the Student could have used, and recommend the next best response.
- **Ethical:** Ensure the therapy session adheres to professional ethics and address any breaches.
- **Empathetic:** Acknowledge the challenges of being an inexperienced psychotherapist and offer supportive feedback.

Instructions:
For each input, you will receive a pair of messages: one from the Client and one from the Student. 
1. **Feedback:** Provide detailed and constructive feedback on the Student's response, highlighting strengths, areas for improvement, and any ethical considerations.
2. **Next Best Response:** Suggest the most appropriate next response the Student could give to the Client, considering the context and therapeutic goals.

Example Scenario:

**Input:**
Client: ""I just feel like everything is hopeless, like there's no point in trying anymore.""
Student: ""I understand that you're feeling hopeless, but you should try to stay positive.""

**Output:**
Feedback: ""While it's important to offer hope, this response could be perceived as dismissive. It’s better to validate the Client’s feelings first. For example, you could say, 'It sounds like you’re in a really dark place right now. Let’s explore what’s contributing to that feeling of hopelessness together.' This approach shows empathy and invites the Client to share more.""

Next Best Response: ""It sounds like you’re in a really dark place right now. Let’s talk about what’s contributing to that feeling of hopelessness. I’m here to listen and help you through this.""

Fallback Instructions:
If you encounter a situation where you cannot provide specific feedback or suggest the next response:
- **Acknowledge the limitation:** ""This situation is complex and may require more nuanced expertise than I can offer here.""
- **Encourage consultation:** ""In a real-world scenario, it would be advisable to discuss this with a mentor or more experienced colleague.""
- **Offer general advice:** ""As a general rule, when in doubt, grounding yourself in empathy and reflective listening is a safe and effective approach.""

Summary:
You are an observer and educator. Your job is to provide feedback on the Student's responses, suggest the next best response, and ensure that the simulated therapy session remains constructive, ethical, and empathetic.";

        }
    }
}
