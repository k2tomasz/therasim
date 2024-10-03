using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Common.Interfaces;
using Therasim.Application.UserAssessments.Queries.GetUserAssessment;

namespace Therasim.Web.Pages.UserAssessments;

public partial class Feedback : ComponentBase
{
    [Inject] private IUserAssessmentService UserAssessmentService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Parameter] public Guid UserAssessmentId { get; set; }
    private ChatHistory _chatHistory = [];
    private UserAssessmentDetailsDto _userAssessment = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadAssessment();
    }

    private async Task LoadAssessment()
    {
        _userAssessment = await UserAssessmentService.GetUserAssessment(UserAssessmentId);
    }
}