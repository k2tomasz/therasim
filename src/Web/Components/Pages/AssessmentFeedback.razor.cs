using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Components.Chat;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Common.Interfaces;
using Therasim.Application.UserAssessments.Queries.GetUserAssessment;

namespace Therasim.Web.Components.Pages;

public partial class AssessmentFeedback : ComponentBase
{
    [Inject] private IUserAssessmentService UserAssessmentService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Parameter] public Guid UserAssessmentId { get; set; }
    //private ChatContainer _chatContainerComponent = null!;
    private ChatHistory _chatHistory = [];
    private UserAssessmentDetailsDto _userAssessment = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadAssessment();
    }

    private async Task LoadAssessment()
    {
        _userAssessment = await UserAssessmentService.GetUserAssessment(UserAssessmentId);
        //if (string.IsNullOrEmpty(_userAssessment.ChatHistory))
        //{
        //    _chatHistory.AddSystemMessage(LanguageModelService.GetSystemPromptForPatient());
        //    return;
        //}

        //var deserializedHistory = JsonSerializer.Deserialize<ChatHistory>(_assessment.ChatHistory);
        //if (deserializedHistory is not null)
        //{
        //    _chatHistory = deserializedHistory;
        //}
    }
}