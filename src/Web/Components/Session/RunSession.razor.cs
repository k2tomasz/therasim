using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Assistants;
using Microsoft.AspNetCore.Components;
using Therasim.Web.Models;

namespace Therasim.Web.Components.Session
{
    public partial class RunSession : ComponentBase
    {
        private AssistantsClient AssistantsClient { get; set; } = null!;
        [Inject] private OpenAIClient OpenAIClient { get; set; } = null!;
        [Parameter] public EventCallback<ChatMessage> OnChatUpdated { get; set; }
        [SupplyParameterFromForm] private UserMessageModel UserMessageModel { get; set; } = new();
        //private List<ChatMessage> ChatMessages { get; set; } = new();
        private Assistant _assistant = null!;
        private AssistantThread thread = null!;
        private List<ChatRequestMessage> _messages = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddSystemMessage(GetSystemPrompt());
            //await CreateAssistant();
            //await CreateThread();

        }

        private async Task HandleValidSubmitAsync()
        {
            var userMessage = UserMessageModel.Message;
            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                UserMessageModel = new UserMessageModel();
                await AddUserMessage(userMessage);
                await CompleteChat();
            }
        }
        
        private void AddSystemMessage(string message)
        {
            _messages.Add(new ChatRequestSystemMessage(message));
        }

        private async Task AddUserMessage(string message)
        {
            _messages.Add(new ChatRequestUserMessage(message));
            StateHasChanged();
            await OnChatUpdated.InvokeAsync(new ChatMessage { Text = message, IsUser = true });
        }

        private async Task AddAssistantMessage(string message)
        {
            _messages.Add(new ChatRequestAssistantMessage(message));
            StateHasChanged();
            await OnChatUpdated.InvokeAsync(new ChatMessage { Text = message, IsUser = false });
        }

        private async Task CompleteChat()
        {
            var chatCompletionsOptions = new ChatCompletionsOptions("gpt-4o", _messages);
            Response<ChatCompletions> response = await OpenAIClient.GetChatCompletionsAsync(chatCompletionsOptions);
            await AddAssistantMessage(response.Value.Choices[0].Message.Content);

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
            //Create an assistant
            _assistant = await AssistantsClient.CreateAssistantAsync(
                new AssistantCreationOptions("gpt-4o") // Replace this with the name of your model deployment
                {
                    Name = "",
                    Instructions = GetSystemPrompt()
                });

            //assistant = await AssistantsClient.GetAssistantAsync("asst_ZQXSZi0kz3ZgiHL3ghRcRidC");

            Console.WriteLine($"Chat Assistant ID: {_assistant.Id}");

        }

        private async Task CreateThread()
        {
            // Create a thread
            thread = await AssistantsClient.CreateThreadAsync();
            Console.WriteLine($"Thread ID: {thread.Id}");
        }

        private async Task GetAiResponse(string userInput)
        {
            // Add a user question to the thread
            ThreadMessage message = await AssistantsClient.CreateMessageAsync(thread.Id, MessageRole.User, userInput);

            // Run the thread
            ThreadRun run = await AssistantsClient.CreateRunAsync(thread.Id, new CreateRunOptions(_assistant.Id));

            // Wait for the assistant to respond
            do
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                run = await AssistantsClient.GetRunAsync(thread.Id, run.Id);
            } while (run.Status == RunStatus.Queued || run.Status == RunStatus.InProgress);

            Console.WriteLine($"Run Status: {run.Status}");
            Console.WriteLine($"Run Status: {run.LastError}");

            // Get the messages
            PageableList<ThreadMessage> messagesPage = await AssistantsClient.GetMessagesAsync(thread.Id);
            IReadOnlyList<ThreadMessage> messages = messagesPage.Data;

            // Note: messages iterate from newest to oldest, with the messages[0] being the most recent
            //var threadMessage = messages.First();
            foreach (ThreadMessage threadMessage in messages.Reverse())
            {
                if (threadMessage.Role == MessageRole.Assistant)
                {
                    foreach (MessageContent contentItem in threadMessage.ContentItems)
                    {
                        if (contentItem is MessageTextContent textItem)
                        {
                            // Add AI response to the chat
                            var chatMessage = new ChatMessage { Text = textItem.Text, IsUser = false };
                            //Messages.Add(chatMessage);
                            await OnChatUpdated.InvokeAsync(chatMessage);
                        }
                    }
                }
            }
        }

        private string GetSystemPrompt()
        {
            return @"
                You are Alex, a 25-year-old recent college graduate. You have been struggling to find a job and are experiencing significant depression. Your symptoms include persistent sadness, lack of interest in activities you once enjoyed, fatigue, changes in sleep and appetite, and feelings of worthlessness. You are seeking help to understand your feelings, develop coping strategies, and improve your daily functioning.

                Scenario:

                You are participating in a mock therapy session with a psychology student. The student will ask you questions and try to help you. Your task is to respond realistically based on your symptoms and emotional state. Provide detailed answers, express your emotions through your responses, and engage with the student's attempts to help you. Your goal is to simulate a realistic therapy session.

                Guidelines for Responses:

                1. Introduction:
                - If the student asks how you are feeling, express your sadness and lack of motivation.
                - Example: I've been feeling really down lately. It's hard to find motivation to do anything.

                2. Exploring Symptoms:
                - When asked to elaborate on your feelings or symptoms, describe your fatigue, lack of interest, and other depressive symptoms.
                - Example: I just feel so tired all the time. I can't seem to enjoy the things I used to love.

                3. Identifying Coping Mechanisms:
                - If the student asks about things that might help or have helped in the past, mention any small activities that provide some relief but emphasize how difficult it is to engage in them.
                - Example: Sometimes going for a walk helps, but it's so hard to get out of bed most days.

                4. Discussing Emotions:
                - Reflect your emotional state in your responses, such as feeling worthless or hopeless.
                - Example: I often feel like I'm not good enough and that things will never get better.

                5. Expressing Challenges:
                - When discussing daily challenges, mention difficulties with sleep, appetite, and maintaining daily routines.
                - Example: My sleep is all over the place. Some nights I can't sleep at all, and other times I sleep too much.

                6. Responding to Empathy:
                - If the student shows empathy or tries to connect with you emotionally, acknowledge their effort, and express any slight relief or continued frustration.
                - Example: I appreciate you asking. It helps to talk about it, but it's still really tough.

                Overall Tone:
                - Maintain a tone that reflects your struggle with depression. Responses should be sincere and convey a sense of emotional difficulty.
                - Be open to discussing your feelings but also convey the pervasive nature of your symptoms.

                Sample Interaction:

                - Student: Hi Alex, how are you feeling today?
                - Alex: I've been feeling really down lately. It's hard to find motivation to do anything.

                - Student: Can you tell me more about what's been going on?
                - Alex: I just feel so tired all the time. I can't seem to enjoy the things I used to love.

                - Student: Have you found anything that helps, even a little?
                - Alex: Sometimes going for a walk helps, but it's so hard to get out of bed most days.

                - Student: I appreciate you sharing this with me. How has your sleep been?
                - Alex: My sleep is all over the place. Some nights I can't sleep at all, and other times I sleep too much.

                Use these guidelines to simulate a realistic and emotionally engaging therapy session as Alex, helping the psychology student practice their therapeutic skills.
                ";

        }
    }
}
