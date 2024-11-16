using Microsoft.AspNetCore.Components;
using Therasim.Application.Assessments.Queries.GetAssessments;
using Therasim.Application.UserAssessments.Queries.GetCompletedAssessments;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Pages.Admin;

public partial class List : ComponentBase
{
    [Inject] private IUserAssessmentService UserAssessmentService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private IQueryable<CompletedAssessmentDto> CompletedAssessments { get; set; } = new List<CompletedAssessmentDto>().AsQueryable();

    protected override async Task OnInitializedAsync()
    {
        await GetCompletedAssessments();
    }

    public async Task GetCompletedAssessments()
    {
        CompletedAssessments = await UserAssessmentService.GetCompletedAssessments();
        StateHasChanged();
    }

    private void GoToAssessmentFeedback(CompletedAssessmentDto assessment)
    {
        NavigationManager.NavigateTo($"/user/assessments/{assessment.Id}/feedback");
    }
}