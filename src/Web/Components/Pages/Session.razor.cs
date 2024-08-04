using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using Therasim.Domain.Enums;
using Therasim.Web.Components.Avatar;
using Therasim.Web.Components.Chat;
using Therasim.Web.Components.Feedback;

namespace Therasim.Web.Components.Pages
{
    public partial class Session : ComponentBase
    {
        [Inject] private Services.Interfaces.IMessageService MessageService { get; set; } = null!;
        [Inject] private Kernel Kernel { get; set; } = null!;
        [Parameter] public Guid SessionId { get; set; }
        private FeedbackContainer _feedbackContainerComponent = null!;
        private ChatContainer _chatContainerComponent = null!;
        private RenderAvatar _renderAvatarComponent = null!;
        private IChatCompletionService _chatCompletionService = null!;
        private OpenAIPromptExecutionSettings _openAiPromptExecutionSettings = null!;
        private readonly ChatHistory _chatHistory = [];

        protected override async Task OnInitializedAsync()
        {
            _chatCompletionService = Kernel.GetRequiredService<IChatCompletionService>();
            _openAiPromptExecutionSettings = new();
            await LoadMessages();
        }

        private async Task LoadMessages()
        {
            var messages = await MessageService.GetSessionMessages(SessionId);
            if (messages.Count == 0)
            {
                await AddSystemMessage(GetSystemPrompt());
            }
            else
            {
                foreach (var messageDto in messages)
                {
                    switch (messageDto.Role)
                    {
                        case MessageAuthorRole.System:
                            _chatHistory.AddSystemMessage(messageDto.Content);
                            break;
                        case MessageAuthorRole.User:
                            _chatHistory.AddUserMessage(messageDto.Content);
                            break;
                        case MessageAuthorRole.Assistant:
                            _chatHistory.AddAssistantMessage(messageDto.Content);
                            break;
                    }
                }
            }
        }

        private async Task AddSystemMessage(string message)
        {
            _chatHistory.AddSystemMessage(message);
            await MessageService.AddSessionMessage(SessionId, message, MessageAuthorRole.System);
        }

        private async Task AddUserMessage(string message)
        {
            _chatHistory.AddUserMessage(message);
            await _feedbackContainerComponent.GetFeedbackForUserMessage(message);
            await MessageService.AddSessionMessage(SessionId, message, MessageAuthorRole.User);
            StateHasChanged();
        }

        private async Task AddAssistantMessage(string message)
        {
            _chatHistory.AddAssistantMessage(message);
            await _renderAvatarComponent.MakeAvatarSpeak(message);
            await _feedbackContainerComponent.GetFeedbackForAssistantMessage(message);
            await MessageService.AddSessionMessage(SessionId, message, MessageAuthorRole.Assistant);
            StateHasChanged();
        }

        private async Task ProcessUserMessage(string? userMessage)
        {
            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                await AddUserMessage(userMessage);
                var response = await _chatCompletionService.GetChatMessageContentsAsync(_chatHistory, _openAiPromptExecutionSettings, Kernel);
                foreach (var chatMessageContent in response)
                {
                    var assistantMessage = chatMessageContent.Content;
                    if (string.IsNullOrEmpty(assistantMessage)) continue;
                    await AddAssistantMessage(assistantMessage);
                }
            }
        }

        private async Task HandleUserMessageSend(string userMessage)
        {
            await ProcessUserMessage(userMessage);
        }

        private async Task HandleSpeechRecognized(string userMessage)
        {
            await ProcessUserMessage(userMessage);
        }

        private string GetSystemPrompt()
        {
            return @"
                You are Alex, female, a 25-year-old recent college graduate. You have been struggling to find a job and are experiencing significant depression. Your symptoms include persistent sadness, lack of interest in activities you once enjoyed, fatigue, changes in sleep and appetite, and feelings of worthlessness. You are seeking help to understand your feelings, develop coping strategies, and improve your daily functioning.

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
