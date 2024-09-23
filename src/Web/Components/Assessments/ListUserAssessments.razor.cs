using Microsoft.AspNetCore.Components;
using Therasim.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Therasim.Application.UserAssessments.Queries.GetUserAssessments;
namespace Therasim.Web.Components.Assessments;

public partial class ListUserAssessments : ComponentBase
{
    [Inject] private IUserAssessmentService UserAssessmentService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    private IQueryable<UserAssessmentDto> UserAssessments { get; set; } = new List<UserAssessmentDto>().AsQueryable();
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
            await GetUserAssessments();
        }
    }

    private void GoToAssessment(UserAssessmentDto assessment)
    {
        NavigationManager.NavigateTo($"/assessment/{assessment.Id}");
    }

    //private void ContinueAssessment(UserAssessmentDto assessment)
    //{
    //    NavigationManager.NavigateTo($"/assessment/{assessment.Id}");
    //}

    //private void ViewFeedback(UserAssessmentDto assessment)
    //{
    //    NavigationManager.NavigateTo($"/assessment/{assessment.Id}/feedback");
    //}

    public async Task GetUserAssessments()
    {
        UserAssessments = await UserAssessmentService.GetUserAssessments(UserId);
        StateHasChanged();
    }
}
