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
    private bool _micOn;
    private Icon _micIcon = new Icons.Regular.Size32.Mic();
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
        _micOn = !_micOn;
        var functionName = _micOn ? "startContinuousRecognitionAsync" : "stopContinuousRecognitionAsync";
        _micIcon = _micOn ? new Icons.Filled.Size32.Mic() : new Icons.Regular.Size32.Mic();
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