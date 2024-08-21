using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Models;

namespace Therasim.Web.Components.Chat
{
    public partial class ChatContainer : ComponentBase, IAsyncDisposable
    {
        [Parameter] public EventCallback<string> OnUserMessageSend { get; set; }
        [Parameter] public EventCallback<string> OnSpeechRecognized { get; set; }
        [Parameter] public ChatHistory ChatHistory { get; set; } = [];
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
            if (firstRender)
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
            var functionName = _micOn ? "startContinuousRecognitionAsync" : "stopContinuousRecognitionAsync";
            _micIcon = _micOn ? new Icons.Filled.Size16.Mic() : new Icons.Regular.Size16.Mic();
            await _speechModule.InvokeVoidAsync(functionName, _objRef);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_speechModule is not null)
            {
                await _speechModule.DisposeAsync();
                _objRef?.Dispose();
            }
        }
    }
}
