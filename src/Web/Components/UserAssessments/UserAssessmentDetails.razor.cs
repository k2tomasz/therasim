using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Therasim.Application.UserAssessments.Queries.GetUserAssessment;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.UserAssessments;

public partial class UserAssessmentDetails : ComponentBase
{
    [Inject] private IUserAssessmentService UserAssessmentService { get; set; } = null!;
    [Inject] private IUserAssessmentTaskService UserAssessmentTaskService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    [Parameter] public Guid UserAssessmentId { get; set; }
    private UserAssessmentDetailsDto? _assessment = null!;

    protected override async Task OnInitializedAsync()
    {
        await GetUserAssessment();
    }

    private async Task GetUserAssessment()
    {
        _assessment = await UserAssessmentService.GetUserAssessment(UserAssessmentId);
        StateHasChanged();
    }

    private void StartNextTask()
    {
        if (_assessment == null) return;
        NavigationManager.NavigateTo($"/user/assessments/{UserAssessmentId}/tasks/{_assessment.NextUserAssessmentTaskId}");
    }
}
