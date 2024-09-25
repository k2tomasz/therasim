using Microsoft.AspNetCore.Components;
using Therasim.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Therasim.Application.Assessments.Queries.GetAssessments;

namespace Therasim.Web.Components.Assessments;

public partial class ListAssessments : ComponentBase
{
    [Inject] private IAssessmentService AssessmentService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    private IQueryable<AssessmentDto> Assessments { get; set; } = new List<AssessmentDto>().AsQueryable();
    private string UserId { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        if (AuthenticationState is not null)
        {
            var state = await AuthenticationState;
            var principal = state.User;
            if (principal.Identity?.IsAuthenticated == true)
            {
                UserId = state.User.Claims
                    .Where(c => c.Type.Equals(@"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
                    .Select(c => c.Value)
                    .FirstOrDefault() ?? string.Empty;
            }
            await GetAssessments();
        }
    }

    private void GoToAssessment(AssessmentDto assessment)
    {
        NavigationManager.NavigateTo($"/assessment/{assessment.Id}");
    }

    public async Task GetAssessments()
    {
        Assessments = (await AssessmentService.GetAssessments()).AsQueryable();
        StateHasChanged();
    }
}
