using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Domain.Enums;
using Therasim.Web.Models;

namespace Therasim.Web.Components.Chat
{
    public partial class ChatContainer : ComponentBase, IAsyncDisposable
    {
        [Parameter] public EventCallback<string> OnUserMessageSend { get; set; }
        [Parameter] public EventCallback<string> OnSpeechRecognized { get; set; }
        [Parameter] public EventCallback OnTimerElapsed { get; set; }
        [Parameter] public ChatHistory ChatHistory { get; set; } = [];
        [Parameter] public bool ReadOnly { get; set; } = false;
        [Parameter] public Language Language { get; set; } = Language.English; 
        [SupplyParameterFromForm] private UserMessageModel UserMessageModel { get; set; } = new();
        [Inject] private IJSRuntime JS { get; set; } = null!;
        private DotNetObjectReference<ChatContainer>? _objRef;
        private IJSObjectReference? _speechModule;
        private bool _micOn;
        private Icon _micIcon = new Icons.Regular.Size16.Mic();

        protected override void OnInitialized()
        {
            _objRef = DotNetObjectReference.Create(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !ReadOnly)
            {
                _speechModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Chat/ChatContainer.razor.js");
                await _speechModule.InvokeVoidAsync("initializeSpeechRecognition");
            }
        }

        private async Task HandleValidSubmitAsync()
        {
            var userMessage = UserMessageModel.Message;
            if (string.IsNullOrWhiteSpace(userMessage)) return;
            UserMessageModel = new UserMessageModel();
            await OnUserMessageSend.InvokeAsync(userMessage);
        }

        [JSInvokable]
        public async Task AddUserMessageFromSpeech(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            await OnSpeechRecognized.InvokeAsync(message);
        }

        private async Task StartSpeechRecognition(MouseEventArgs obj)
        {
            if (_speechModule is null) return;
            _micOn = !_micOn;
            var language = Language == Language.English ? "en-US" : "pl-PL";
            if (_micOn) await _speechModule.InvokeVoidAsync("startContinuousRecognitionAsync", _objRef, language);
            else await _speechModule.InvokeVoidAsync("stopContinuousRecognitionAsync", _objRef);
            _micIcon = _micOn ? new Icons.Filled.Size16.Mic() : new Icons.Regular.Size16.Mic();
        }

        public async Task StartTimer(int timeInMinutes)
        {
            if (timeInMinutes <= 0) return;
            if (_speechModule is not null)
            {
                await _speechModule.InvokeVoidAsync("startTimer", _objRef, timeInMinutes);
            }
        }

        [JSInvokable]
        public async Task OnTimerElapsedCallback()
        {
            if (OnTimerElapsed.HasDelegate)
            {
                await OnTimerElapsed.InvokeAsync();
            }
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_speechModule != null)
            {
                await _speechModule.InvokeVoidAsync("stopTimer");
                await _speechModule.DisposeAsync();
                _objRef?.Dispose();
            }
        }
    }
}
