using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Common.Interfaces;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTasksFeedback;

namespace Therasim.Web.Pages.UserAssessments;

public partial class Feedback : ComponentBase
{
    [Inject] private IUserAssessmentTaskService UserAssessmentTaskService { get; set; } = null!;
    [Inject] private IUserAssessmentService UserAssessmentService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Parameter] public Guid UserAssessmentId { get; set; }
    private ChatHistory _chatHistory = [];
    private IList<UserAssessmentTaskFeedbackDto>? _userAssessmentTasks;

    protected override async Task OnInitializedAsync()
    {
        await GenerateUserAssessmentFeedback();
        await GetAssessmentTasks();
    }

    private async Task GenerateUserAssessmentFeedback()
    {
        await UserAssessmentService.GenerateUserAssessmentFeedback(UserAssessmentId);
    }

    public async Task GetAssessmentTasks()
    {
        _userAssessmentTasks = await UserAssessmentTaskService.GetAssessmentTasksFeedback(UserAssessmentId);
        StateHasChanged();
    }
}