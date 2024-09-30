using Microsoft.AspNetCore.Components;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTasks;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.UserAssessmentTasks;

public partial class UserAssessmentTaskList : ComponentBase
{
    [Inject] private IUserAssessmentTaskService UserAssessmentTaskService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Parameter] public Guid UserAssessmentId { get; set; }
    private IList<UserAssessmentTaskDto> _userAssessmentTasks = new List<UserAssessmentTaskDto>();

    protected override async Task OnParametersSetAsync()
    {
        await GetAssessmentTasks();
    }

    public async Task GetAssessmentTasks()
    {
        _userAssessmentTasks = await UserAssessmentTaskService.GetUserAssessmentTasks(UserAssessmentId);
        StateHasChanged();
    }

    private void GoToAssessmentTaskSession(UserAssessmentTaskDto userAssessmentTask)
    {
        NavigationManager.NavigateTo($"/user/assessments/{UserAssessmentId}/tasks/{userAssessmentTask.Id}");
    }

    private void GoToAssessmentTaskFeedback(UserAssessmentTaskDto userAssessmentTask)
    {
        NavigationManager.NavigateTo($"/user/assessments/{UserAssessmentId}/tasks/{userAssessmentTask.Id}/feedback");
    }
}
