using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Components.Avatar;
using Therasim.Web.Components.Chat;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Common.Interfaces;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;
using Microsoft.FluentUI.AspNetCore.Components;
using Therasim.Web.Components.UserAssessmentTasks;

namespace Therasim.Web.Pages.UserAssessmentTasks;

public partial class TaskSession : ComponentBase
{
    [Inject] private IAssessmentService AssessmentService { get; set; } = null!;
    [Inject] private IUserAssessmentTaskService UserAssessmentTaskService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Parameter] public Guid UserAssessmentId { get; set; }
    [Parameter] public Guid UserAssessmentTaskId { get; set; }
    private ChatContainer _chatContainerComponent = null!;
    private RenderAvatar _renderAvatarComponent = null!;
    private ChatHistory _chatHistory = [];
    private UserAssessmentTaskDetailsDto _userAssessmentTask = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadAssessment();
        await OpenStartDialogAsync();
    }

    private async Task LoadAssessment()
    {
        _userAssessmentTask = await UserAssessmentTaskService.GetUserAssessmentTask(UserAssessmentTaskId);
        if (string.IsNullOrEmpty(_userAssessmentTask.ChatHistory))
        {
            _chatHistory.AddSystemMessage(_userAssessmentTask.ClientSystemPrompt);
            _chatHistory.AddAssistantMessage(_userAssessmentTask.ClientInitialDialogue);
            return;
        }

        var deserializedHistory = JsonSerializer.Deserialize<ChatHistory>(_userAssessmentTask.ChatHistory);
        if (deserializedHistory is not null)
        {
            _chatHistory = deserializedHistory;
        }
        else
        {
            _chatHistory.AddSystemMessage(_userAssessmentTask.ClientSystemPrompt);
            _chatHistory.AddAssistantMessage(_userAssessmentTask.ClientInitialDialogue);
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

    private async Task EndAssessmentTask()
    {
        //StateHasChanged();
        await SaveChatHistory();
        await UserAssessmentTaskService.EndAssessmentTask(UserAssessmentTaskId);
        //await UserAssessmentTaskService.GenerateAssessmentTaskFeedback(UserAssessmentTaskId);
        //NavigationManager.NavigateTo($"/assessments/{UserAssessmentId}");
    }

    private async Task OpenStartDialogAsync()
    {

        await DialogService.ShowDialogAsync<StartAssessmentTaskDialog>(_userAssessmentTask, new DialogParameters()
        {
            Title = "Start Assessment Task",
            PreventDismissOnOverlayClick = true,
            OnDialogResult = DialogService.CreateDialogCallback(this, HandleStartDialog),
            PrimaryAction = "Start",
            SecondaryAction = "Back",
            ShowDismiss = false,
            Modal = true
        });
    }

    private async Task OpenEndDialogAsync()
    {

        await DialogService.ShowDialogAsync<EndAssessmentTaskDialog>(_userAssessmentTask, new DialogParameters()
        {
            Title = "Assessment Task Completed",
            PreventDismissOnOverlayClick = true,
            OnDialogResult = DialogService.CreateDialogCallback(this, HandleEndDialog),
            PrimaryAction = "See Feedback",
            SecondaryAction = "Back to Assessment",
            ShowDismiss = false,
            Modal = true
        });
    }

    private async Task HandleStartDialog(DialogResult result)
    {
        if (result.Cancelled)
        {
            NavigationManager.NavigateTo($"user/assessments/{UserAssessmentId}");
            return;
        }

        await UserAssessmentTaskService.StartAssessmentTask(UserAssessmentTaskId);
        _chatContainerComponent.StartTimer(_userAssessmentTask.LengthInMinutes ?? 2);
    }

    private Task HandleEndDialog(DialogResult result)
    {
        if (result.Cancelled)
        {
            NavigationManager.NavigateTo($"user/assessments/{UserAssessmentId}");
            return Task.CompletedTask;
        }

        NavigationManager.NavigateTo($"user/assessments/{UserAssessmentId}/tasks/{UserAssessmentTaskId}/feedback");
        return Task.CompletedTask;
    }

    private async Task HandleOnTimerElapsed()
    {
        await EndAssessmentTask();
        await OpenEndDialogAsync();
    }
}