using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Components.Avatar;
using Therasim.Web.Components.Chat;
using Therasim.Web.Components.Feedback;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Common.Interfaces;

namespace Therasim.Web.Components.Pages;

public partial class Session : ComponentBase
{
    [Inject] private ISessionService SessionService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Parameter] public Guid SessionId { get; set; }
    private FeedbackContainer _feedbackContainerComponent = null!;
    private ChatContainer _chatContainerComponent = null!;
    private RenderAvatar _renderAvatarComponent = null!;
    private ChatHistory _chatHistory = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadSession();
    }
    private async Task LoadSession()
    {
        var session = await SessionService.GetSession(SessionId);
        if (string.IsNullOrEmpty(session.ChatHistory))
        {
            _chatHistory.AddSystemMessage(LanguageModelService.GetSystemPromptForPatient());
            return;
        }

        var deserializedHistory = JsonSerializer.Deserialize<ChatHistory>(session.ChatHistory);
        if (deserializedHistory is not null)
        {
            _chatHistory = deserializedHistory;
        }
        else
        {
            _chatHistory.AddSystemMessage(LanguageModelService.GetSystemPromptForPatient());
        }
    }

    private async Task AddUserMessage(string message)
    {
        _chatHistory.AddUserMessage(message);
        await SaveChatHistory();
        StateHasChanged();
    }

    private async Task AddAssistantMessage(string message)
    {
        _chatHistory.AddAssistantMessage(message);
        await _renderAvatarComponent.MakeAvatarSpeak(message);
        await SaveChatHistory();
        StateHasChanged();
    }

    private async Task ProcessUserMessage(string? userMessage)
    {
        if (string.IsNullOrEmpty(userMessage)) return;
        await AddUserMessage(userMessage);
        var response = await LanguageModelService.GetChatMessageContentsAsync(_chatHistory);
        if (string.IsNullOrEmpty(response)) return;
        await AddAssistantMessage(response);
        await _feedbackContainerComponent.GetFeedback(userMessage, response);
    }

    private async Task SaveChatHistory()
    {
        var chatHistoryJson = JsonSerializer.Serialize(_chatHistory);
        await SessionService.SaveChatHistory(SessionId, chatHistoryJson);
    }

    private async Task HandleUserMessageSend(string userMessage)
    {
        await ProcessUserMessage(userMessage);
    }

    private async Task HandleSpeechRecognized(string userMessage)
    {
        await ProcessUserMessage(userMessage);
    }
}

