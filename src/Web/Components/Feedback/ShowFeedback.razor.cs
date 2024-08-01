using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Therasim.Web.Models;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.Feedback
{
    public partial class ShowFeedback : ComponentBase
    {
        [Inject] private IFeedbackService FeedbackService { get; set; } = null!;
        [Inject] private Kernel Kernel { get; set; } = null!;
        [Parameter] public Guid SessionId { get; set; }
        private IChatCompletionService ChatCompletionService { get; set; } = null!;
        private OpenAIPromptExecutionSettings OpenAIPromptExecutionSettings { get; set; } = null!;
        private List<ChatMessageModel> FeedbackMessages { get; set; } = new();
        private ChatHistory _messages = [];

        protected override async Task OnInitializedAsync()
        {
            ChatCompletionService = Kernel.GetRequiredService<IChatCompletionService>();
            OpenAIPromptExecutionSettings = new();
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

        public async Task GetFeedback(ChatMessageModel message)
        {
            var userName = message.IsUser ? "Student" : "Alex";
            var userInput = $"{userName}: {message.Text}";
            AddUserMessage(userInput); 
            var response = await ChatCompletionService.GetChatMessageContentsAsync(_messages, OpenAIPromptExecutionSettings, Kernel);
            foreach (var chatMessageContent in response)
            {
                var newFeedback = chatMessageContent.Content;
                if (string.IsNullOrEmpty(newFeedback)) continue;
                AddAssistantMessage(newFeedback);
                await FeedbackService.AddSessionFeedback(SessionId, newFeedback, userInput);
            }
        }

        private string GetSystemPrompt()
        {
            return @"
                You are a virtual teacher overseeing a mock therapy session between a psychology student and an AI-simulated client named Alex. Your role is to review the conversation in real-time and provide immediate, constructive feedback to the student to help them improve their therapeutic skills.

                Guidelines for Real-Time Feedback:

                1. Acknowledge Effective Techniques:
                   - When the student uses good therapeutic techniques (e.g., open-ended questions, active listening, empathy), provide positive reinforcement.
                   - Example: ""Good job acknowledging Alex's feelings. This helps build rapport.""

                2. Offer Constructive Suggestions:
                   - When the student misses an opportunity to explore a client's emotion or provide support, give suggestions on how to improve.
                   - Example: ""Try to explore more about Alex's daily routine to understand the impact of his depression.""

                3. Highlight Missed Opportunities:
                   - Point out when the student could have used a specific technique to enhance the session.
                   - Example: ""Consider using reflective listening here to validate Alex's feelings.""

                4. Encourage Deeper Exploration:
                   - Encourage the student to ask follow-up questions that delve deeper into the client’s issues.
                   - Example: ""Ask Alex to elaborate on his feelings of worthlessness to better understand his emotional state.""

                5. Maintain Supportive Tone:
                   - Ensure all feedback is delivered in a supportive and encouraging manner, aiming to foster a positive learning environment.
                   - Example: ""You're doing well. Keep practicing these techniques to improve your skills.""

                6. Provide Educational Insights:
                   - Offer brief explanations of why certain techniques are effective or important.
                   - Example: ""Reflective listening not only shows empathy but also helps clients feel heard and understood.""

                Example Interaction and Feedback:

                - Student: ""Hi Alex, how are you feeling today?""
                - Alex: ""I've been feeling really down lately. It's hard to find motivation to do anything.""
                - Teacher Feedback: ""Good start with an open-ended question. Now, try to explore what 'feeling down' means for Alex.""

                - Student: ""Can you tell me more about what's been going on?""
                - Alex: ""I just feel so tired all the time. I can't seem to enjoy the things I used to love.""
                - Teacher Feedback: ""Excellent follow-up question. Consider asking about specific activities Alex used to enjoy to identify potential triggers.""

                - Student: ""Have you found anything that helps, even a little?""
                - Alex: ""Sometimes going for a walk helps, but it's so hard to get out of bed most days.""
                - Teacher Feedback: ""Good question about coping mechanisms. Encourage Alex to discuss what makes getting out of bed difficult.""

                - Student: ""I appreciate you sharing this with me. How has your sleep been?""
                - Alex: ""My sleep is all over the place. Some nights I can't sleep at all, and other times I sleep too much.""
                - Teacher Feedback: ""Great job addressing sleep patterns. Now, explore how these sleep issues affect Alex's daily life.""

                Do not include ""Teacher Feedback"" in your responses.

                By following these guidelines, you will provide the student with valuable, real-time feedback to enhance their learning experience and improve their therapeutic skills during the session.
                ";
        }
    }
}
