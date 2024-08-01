using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Therasim.Web.Models;
using AuthorRole = Therasim.Domain.Enums.AuthorRole;

namespace Therasim.Web.Components.Chat
{
    public partial class ShowChat : ComponentBase, IAsyncDisposable, IDisposable
    {
        [Inject] private Services.Interfaces.IMessageService MessageService { get; set; } = null!;
        [Inject] private Kernel Kernel { get; set; } = null!;
        [Inject] private IJSRuntime JS { get; set; } = null!;
        [Parameter] public EventCallback<ChatMessageModel> OnChatUpdated { get; set; }
        [Parameter] public Guid SessionId { get; set; }
        [SupplyParameterFromForm] private UserMessageModel UserMessageModel { get; set; } = new();
        
        private IChatCompletionService _chatCompletionService = null!;
        private OpenAIPromptExecutionSettings _openAiPromptExecutionSettings = null!;
        private ChatHistory _messages = [];
        private IJSObjectReference? _speechModule;
        private DotNetObjectReference<ShowChat>? _objRef;
        private bool _isListening;
        private Icon _micIcon = new Icons.Regular.Size16.Mic();
        protected override async Task OnInitializedAsync()
        {
            _objRef = DotNetObjectReference.Create(this);
            _chatCompletionService = Kernel.GetRequiredService<IChatCompletionService>();
            _openAiPromptExecutionSettings = new();
            await LoadMessages();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _speechModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Session/RunSession.razor.js");
                await _speechModule.InvokeVoidAsync("initializeSpeech");
            }
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
                        case AuthorRole.System:
                            _messages.AddSystemMessage(messageDto.Content);
                            break;
                        case AuthorRole.User:
                            _messages.AddUserMessage(messageDto.Content);
                            break;
                        case AuthorRole.Assistant:
                            _messages.AddAssistantMessage(messageDto.Content);
                            break;
                    }
                }
            }
        }

        private async Task HandleValidSubmitAsync()
        {
            var userMessage = UserMessageModel.Message;
            await ProcessUserMessage(userMessage);
        }
        
        private async Task ProcessUserMessage(string? userMessage)
        {
            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                UserMessageModel = new UserMessageModel();
                await AddUserMessage(userMessage);
                var response = await _chatCompletionService.GetChatMessageContentsAsync(_messages, _openAiPromptExecutionSettings, Kernel);
                foreach (var chatMessageContent in response)
                {
                    var newMessage = chatMessageContent.Content;
                    if (string.IsNullOrEmpty(newMessage)) continue;
                    await AddAssistantMessage(newMessage);
                }
            }
        }
        
        private async Task AddSystemMessage(string message)
        {
            _messages.AddSystemMessage(message);
            await MessageService.AddSessionMessage(SessionId, message, AuthorRole.System);
        }

        private async Task AddUserMessage(string message)
        {
            _messages.AddUserMessage(message);
            StateHasChanged();
            await MessageService.AddSessionMessage(SessionId, message, AuthorRole.User);
            await OnChatUpdated.InvokeAsync(new ChatMessageModel { Text = message, IsUser = true });
        }

        private async Task AddAssistantMessage(string message)
        {
            _messages.AddAssistantMessage(message);
            StateHasChanged();
            await MessageService.AddSessionMessage(SessionId, message, AuthorRole.Assistant);
            await OnChatUpdated.InvokeAsync(new ChatMessageModel { Text = message, IsUser = false });
        }

        [JSInvokable]
        public async Task AddUserMessageFromSpeech(string message)
        {
            await ProcessUserMessage(message);
        }

        private async Task StartStopContinuousRecognitionAsync(MouseEventArgs obj)
        {
            if (_speechModule is null) return;
            var functionName = _isListening ? "stopContinuousRecognitionAsync" : "startContinuousRecognitionAsync";
            _micIcon = _isListening ? new Icons.Regular.Size16.Mic() : new Icons.Filled.Size16.Mic();
            await _speechModule.InvokeVoidAsync(functionName, _objRef);
            _isListening = !_isListening;
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

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_speechModule is not null)
            {
                await _speechModule.DisposeAsync();
            }
        }

        public void Dispose()
        {
            if (_speechModule is IDisposable speechModuleDisposable)
                speechModuleDisposable.Dispose();
            else if (_speechModule != null)
                _ = _speechModule.DisposeAsync().AsTask();
            _objRef?.Dispose();
        }
    }
}
