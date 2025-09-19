using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Application.Common.Interfaces;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Pages.UserAssessmentTasks;

public partial class Feedback : ComponentBase
{
    [Inject] private IUserAssessmentTaskService UserAssessmentTaskService { get; set; } = null!;
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
    [Parameter] public Guid UserAssessmentId { get; set; }
    [Parameter] public Guid UserAssessmentTaskId { get; set; }
    private ChatHistory _chatHistory = [];
    private UserAssessmentTaskDetailsDto _userAssessmentTask = null!;
    private string? _feedback;

    protected override async Task OnInitializedAsync()
    {
        await LoadAssessment();
    }

    private async Task LoadAssessment()
    {
        _userAssessmentTask = await UserAssessmentTaskService.GetUserAssessmentTask(UserAssessmentTaskId);
        if (string.IsNullOrEmpty(_userAssessmentTask.Feedback))
        {
            _feedback = await UserAssessmentTaskService.GenerateAssessmentTaskFeedback(UserAssessmentTaskId);
        }
        else
        {
            _feedback = _userAssessmentTask.Feedback;
        }

        StateHasChanged();
    }
}