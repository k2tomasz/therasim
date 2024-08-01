using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Therasim.Web.Components.Session;

public partial class SessionControls : ComponentBase, IAsyncDisposable
{
    [Parameter] public EventCallback<string> OnSpeechRecognized { get; set; }
    [Inject] private IJSRuntime JS { get; set; } = null!;
    private DotNetObjectReference<SessionControls>? _objRef;
    private IJSObjectReference? _speechModule;
    private bool _isListening;
    private Icon _micIcon = new Icons.Regular.Size16.Mic();
    protected override void OnInitialized()
    {
        _objRef = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _speechModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Session/SessionControls.razor.js");
            await _speechModule.InvokeVoidAsync("initializeSpeech");
        }
    }

    [JSInvokable]
    public async Task AddUserMessageFromSpeech(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        await OnSpeechRecognized.InvokeAsync(message);
    }

    private async Task StartStopContinuousRecognitionAsync(MouseEventArgs obj)
    {
        if (_speechModule is null) return;
        var functionName = _isListening ? "stopContinuousRecognitionAsync" : "startContinuousRecognitionAsync";
        _micIcon = _isListening ? new Icons.Regular.Size16.Mic() : new Icons.Filled.Size16.Mic();
        await _speechModule.InvokeVoidAsync(functionName, _objRef);
        _isListening = !_isListening;
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