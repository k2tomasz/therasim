using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Therasim.Web.Components.Session;

public partial class SessionControls : ComponentBase, IAsyncDisposable
{
    [Parameter] public EventCallback<string> OnSpeechRecognized { get; set; }
    [Parameter] public EventCallback<bool> OnChatContainerHide { get; set; }
    [Parameter] public EventCallback<bool> OnFeedbackContainerHide { get; set; }
    [Inject] private IJSRuntime JS { get; set; } = null!;
    private DotNetObjectReference<SessionControls>? _objRef;
    private IJSObjectReference? _speechModule;
    private bool _isListening;
    private bool _isChatVisible = true;
    private bool _isFeedbackVisible = true;
    private Icon _micIcon = new Icons.Regular.Size32.Mic();
    private Icon _chatIcon = new Icons.Regular.Size32.Chat();
    private Icon _feedbackIcon = new Icons.Regular.Size32.PersonFeedback();
    protected override void OnInitialized()
    {
        _objRef = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _speechModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Session/SessionControls.razor.js");
            await _speechModule.InvokeVoidAsync("initializeSpeechRecognition");
        }
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
        var functionName = _isListening ? "stopContinuousRecognitionAsync" : "startContinuousRecognitionAsync";
        _micIcon = _isListening ? new Icons.Regular.Size48.Mic() : new Icons.Filled.Size48.Mic();
        await _speechModule.InvokeVoidAsync(functionName, _objRef);
        _isListening = !_isListening;
    }

    private async Task HideChatContainer(MouseEventArgs obj)
    {
        _chatIcon = _isChatVisible ? new Icons.Filled.Size32.Chat() : new Icons.Regular.Size32.Chat();
        _isChatVisible = !_isChatVisible;
        await OnChatContainerHide.InvokeAsync(_isChatVisible);
    }

    private async Task HideFeedbackContainer(MouseEventArgs obj)
    {
        _feedbackIcon = _isFeedbackVisible ? new Icons.Filled.Size32.PersonFeedback() : new Icons.Regular.Size32.PersonFeedback();
        _isFeedbackVisible = !_isFeedbackVisible;
        await OnFeedbackContainerHide.InvokeAsync(_isFeedbackVisible);
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