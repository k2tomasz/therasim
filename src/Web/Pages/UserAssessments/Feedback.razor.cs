using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Common.Interfaces;
using Therasim.Application.UserAssessments.Queries.GetUserAssessment;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTasksFeedback;

namespace Therasim.Web.Pages.UserAssessments;

public partial class Feedback : ComponentBase
{
    [Inject] private IUserAssessmentTaskService UserAssessmentTaskService { get; set; } = null!;
    [Inject] private IUserAssessmentService UserAssessmentService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Parameter] public Guid UserAssessmentId { get; set; }
    private ChatHistory _chatHistory = [];
    private UserAssessmentDetailsDto _userAssessment = null!;
    private IList<UserAssessmentTaskFeedbackDto> _userAssessmentTasks = new List<UserAssessmentTaskFeedbackDto>();

    protected override async Task OnInitializedAsync()
    {
        await GetAssessmentTasks();
    }

    private async Task LoadAssessment()
    {
        _userAssessment = await UserAssessmentService.GetUserAssessment(UserAssessmentId);
    }

    public async Task GetAssessmentTasks()
    {
        _userAssessmentTasks = await UserAssessmentTaskService.GetAssessmentTasksFeedback(UserAssessmentId);
    }
}