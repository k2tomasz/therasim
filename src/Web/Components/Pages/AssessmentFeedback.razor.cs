using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Application.Assessments.Queries.GetAssessment;
using Therasim.Web.Components.Chat;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Common.Interfaces;

namespace Therasim.Web.Components.Pages;

public partial class AssessmentFeedback : ComponentBase
{
    [Inject] private IAssessmentService AssessmentService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Parameter] public Guid AssessmentId { get; set; }
    private ChatContainer _chatContainerComponent = null!;
    private ChatHistory _chatHistory = [];
    private AssessmentDetailsDto _assessment = null!;

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
    }
}