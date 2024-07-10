using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Assistants;
using Microsoft.AspNetCore.Components;
using Therasim.Web.Components.Pages;
using Therasim.Web.Models;

namespace Therasim.Web.Components.Feedback
{
    public partial class Feedback : ComponentBase
    {

        [Inject] private AssistantsClient AssistantsClient { get; set; } = null!;
        [Inject] private OpenAIClient OpenAIClient { get; set; } = null!;
        private List<ChatMessage> FeedbackMessages { get; set; } = new();
        private Assistant assistant = null!;
        private AssistantThread thread = null!;
        private List<ChatRequestMessage> _messages = new();

        protected override void OnInitialized()
        {
            AddSystemMessage(GetSystemPrompt());
        }

        private void AddSystemMessage(string message)
        {
            _messages.Add(new ChatRequestSystemMessage(message));
        }

        private void AddUserMessage(string message)
        {
            _messages.Add(new ChatRequestUserMessage(message));
            StateHasChanged();
        }

        private void AddAssistantMessage(string message)
        {
            _messages.Add(new ChatRequestAssistantMessage(message));
            StateHasChanged();
        }

        private async Task CompleteChat()
        {
            var chatCompletionsOptions = new ChatCompletionsOptions("gpt-4o", _messages);
            Response<ChatCompletions> response = await OpenAIClient.GetChatCompletionsAsync(chatCompletionsOptions);
            AddAssistantMessage(response.Value.Choices[0].Message.Content);

            // await foreach (StreamingChatCompletionsUpdate chatUpdate in client.GetChatCompletionsStreaming(chatCompletionsOptions))
            // {
            //     if (chatUpdate.Role.HasValue)
            //     {
            //         Console.Write($"{chatUpdate.Role.Value.ToString().ToUpperInvariant()}: ");
            //     }
            //     if (!string.IsNullOrEmpty(chatUpdate.ContentUpdate))
            //     {
            //         Console.Write(chatUpdate.ContentUpdate);
            //     }
            // }
        }



        private async Task CreateAssistant()
        {
            // Create an assistant
            assistant = await AssistantsClient.CreateAssistantAsync(
                new AssistantCreationOptions("gpt-35-turbo") // Replace this with the name of your model deployment
                {
                    Name = "TheraSim Teacher",
                    Instructions = GetSystemPrompt()
                });

            //assistant = await AssistantsClient.GetAssistantAsync("asst_Uo6jpDsi1esBwfmpISzwYqXW");

            Console.WriteLine($"Feedback Assistant ID: {assistant.Id}");

        }

        private async Task CreateThread()
        {
            // Create a thread
            thread = await AssistantsClient.CreateThreadAsync();
        }

        private async Task GetAiResponse(ChatMessage input)
        {
            var userName = input.IsUser ? "Student" : "Alex";
            var userInput = $"{userName}: {input.Text}";
            // Add a user question to the thread
            ThreadMessage message = await AssistantsClient.CreateMessageAsync(thread.Id, MessageRole.User, userInput);

            // Run the thread
            ThreadRun run = await AssistantsClient.CreateRunAsync(thread.Id, new CreateRunOptions(assistant.Id));

            // Wait for the assistant to respond
            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                run = await AssistantsClient.GetRunAsync(thread.Id, run.Id);
            }
            while (run.Status == RunStatus.Queued || run.Status == RunStatus.InProgress);

            // Get the messages
            PageableList<ThreadMessage> messagesPage = await AssistantsClient.GetMessagesAsync(thread.Id);
            IReadOnlyList<ThreadMessage> messages = messagesPage.Data;

            // Note: messages iterate from newest to oldest, with the messages[0] being the most recent
            var threadMessage = messages.Reverse().First();
            foreach (MessageContent contentItem in threadMessage.ContentItems)
            {
                if (contentItem is MessageTextContent textItem)
                {
                    // Add AI response to the chat
                    FeedbackMessages.Add(new ChatMessage { Text = textItem.Text, IsUser = false });
                }
            }
        }

        public async Task GetFeedback(ChatMessage message)
        {
            var userName = message.IsUser ? "Student" : "Alex";
            var userInput = $"{userName}: {message.Text}";
            AddUserMessage(userInput);
            await CompleteChat();
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
