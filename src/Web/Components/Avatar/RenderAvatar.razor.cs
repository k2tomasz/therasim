using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Therasim.Web.Components.Avatar;

public partial class RenderAvatar : ComponentBase, IAsyncDisposable, IDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = null!;
    private IJSObjectReference? _avatarModule;
    private DotNetObjectReference<RenderAvatar>? _objRef;

    protected override void OnInitialized()
    {
        _objRef = DotNetObjectReference.Create(this);
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _avatarModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Avatar/RenderAvatar.razor.js");
            await _avatarModule.InvokeVoidAsync("initializeSpeech");
            await _avatarModule.InvokeVoidAsync("startSession", _objRef);
        }
    }
    
    public async ValueTask Speak(string textToSpeak)
    {
        if (_avatarModule is not null)
        {
            await _avatarModule.InvokeVoidAsync("speak", textToSpeak);
        }
    }
    
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_avatarModule is not null)
        {
            await _avatarModule.DisposeAsync();
        }
    }

    public void Dispose()
    {
        if (_avatarModule is IDisposable speechModuleDisposable)
            speechModuleDisposable.Dispose();
        else if (_avatarModule != null)
            _ = _avatarModule.DisposeAsync().AsTask();
        _objRef?.Dispose();
    }
}