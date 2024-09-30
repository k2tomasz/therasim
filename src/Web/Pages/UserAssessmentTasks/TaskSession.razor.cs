using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Components.Avatar;
using Therasim.Web.Components.Chat;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Common.Interfaces;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;

namespace Therasim.Web.Pages.UserAssessmentTasks;

public partial class TaskSession : ComponentBase
{
    [Inject] private IAssessmentService AssessmentService { get; set; } = null!;
    [Inject] private IUserAssessmentTaskService UserAssessmentTaskService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Parameter] public Guid UserAssessmentId { get; set; }
    [Parameter] public Guid UserAssessmentTaskId { get; set; }
    private ChatContainer _chatContainerComponent = null!;
    private RenderAvatar _renderAvatarComponent = null!;
    private ChatHistory _chatHistory = [];
    private UserAssessmentTaskDetailsDto _userAssessmentTask = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadAssessment();
    }

    private async Task LoadAssessment()
    {
        _userAssessmentTask = await UserAssessmentTaskService.GetUserAssessmentTask(UserAssessmentTaskId);
        if (string.IsNullOrEmpty(_userAssessmentTask.ChatHistory))
        {
            _chatHistory.AddSystemMessage(LanguageModelService.GetSystemPromptForPatient());
            return;
        }

        var deserializedHistory = JsonSerializer.Deserialize<ChatHistory>(_userAssessmentTask.ChatHistory);
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
    }

    private async Task SaveChatHistory()
    {
        var chatHistoryJson = JsonSerializer.Serialize(_chatHistory);
        await UserAssessmentTaskService.SaveAssessmentTaskChatHistory(UserAssessmentTaskId, chatHistoryJson);
    }

    private async Task HandleUserMessageSend(string userMessage)
    {
        await ProcessUserMessage(userMessage);
    }

    private async Task HandleSpeechRecognized(string userMessage)
    {
        await ProcessUserMessage(userMessage);
    }

    private async Task EndAssessment()
    {
        StateHasChanged();
        await SaveChatHistory();
        await UserAssessmentTaskService.EndAssessmentTask(UserAssessmentTaskId);
        await UserAssessmentTaskService.GenerateAssessmentTaskFeedback(UserAssessmentTaskId);
        NavigationManager.NavigateTo($"/assessment/{UserAssessmentId}/feedback");
    }
}