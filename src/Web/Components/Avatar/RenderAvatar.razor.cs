using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Therasim.Domain.Enums;

namespace Therasim.Web.Components.Avatar;

public partial class RenderAvatar : ComponentBase, IAsyncDisposable, IDisposable
{
    [Inject] private IJSRuntime JS { get; set; } = null!;
    [Parameter] public Language Language { get; set; } = Language.English;
    private IJSObjectReference? _avatarModule;
    private DotNetObjectReference<RenderAvatar>? _objRef;
    private string speechSynthesisLanguage = "en-US";
    private string speechSynthesisVoiceName = "en-US-AvaMultilingualNeural";


    protected override void OnInitialized()
    {
        _objRef = DotNetObjectReference.Create(this);
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (Language == Language.Polish)
            {
                speechSynthesisLanguage = "pl-PL";
                speechSynthesisVoiceName = "pl-PL-AgnieszkaNeural";
            }
            _avatarModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Avatar/RenderAvatar.razor.js");
            await _avatarModule.InvokeVoidAsync("initializeAvatar");
            await _avatarModule.InvokeVoidAsync("startSession", _objRef, speechSynthesisLanguage, speechSynthesisVoiceName);
        }
    }
    
    public async ValueTask MakeAvatarSpeak(string textToSpeak)
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