using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Components.Avatar;
using Therasim.Web.Components.Chat;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Common.Interfaces;

namespace Therasim.Web.Components.Pages;

public partial class Assessment : ComponentBase
{
    [Inject] private IAssessmentService AssessmentService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Parameter] public Guid AssessmentId { get; set; }
    private ChatContainer _chatContainerComponent = null!;
    private RenderAvatar _renderAvatarComponent = null!;
    private ChatHistory _chatHistory = [];
    private AssessmentDetailsDto _assessment = null!;
    private bool _showGeneratingFeedbackMessage = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadAssessment();
    }

    private async Task LoadAssessment()
    {
        _assessment = await AssessmentService.GetAssessment(AssessmentId);
        if (string.IsNullOrEmpty(_assessment.ChatHistory))
        {
            _chatHistory.AddSystemMessage(LanguageModelService.GetSystemPromptForPatient());
            return;
        }

        var deserializedHistory = JsonSerializer.Deserialize<ChatHistory>(_assessment.ChatHistory);
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
        await AssessmentService.SaveChatHistory(AssessmentId, chatHistoryJson);
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
        _showGeneratingFeedbackMessage = true;
        StateHasChanged();
        await SaveChatHistory();
        await AssessmentService.EndAssessment(_assessment.Id);
        await AssessmentService.GenerateAssessmentFeedback(_assessment.Id);
        NavigationManager.NavigateTo($"/assessment/{_assessment.Id}/feedback");
    }
}