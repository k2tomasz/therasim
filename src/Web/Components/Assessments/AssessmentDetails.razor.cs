using Microsoft.AspNetCore.Components;
using Therasim.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Therasim.Application.Assessments.Queries.GetAssessment;
using Therasim.Domain.Enums;

namespace Therasim.Web.Components.Assessments;

public partial class AssessmentDetails : ComponentBase
{
    [Inject] private IAssessmentService AssessmentService { get; set; } = null!;
    [Inject] private IUserAssessmentService UserAssessmentService { get; set; } = null!;
    [Inject] private IUserAssessmentTaskService UserAssessmentTaskService { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    [Parameter] public Guid AssessmentId { get; set; }
    private AssessmentDetailsDto? _assessment = null!;
    private Language _language = Language.English;
    private string _userId = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        if (AuthenticationState is not null)
        {
            var state = await AuthenticationState;
            var principal = state.User;
            if (principal.Identity?.IsAuthenticated == true)
            {
                _userId = state.User.Claims
                    .Where(c => c.Type.Equals(@"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
                    .Select(c => c.Value)
                    .FirstOrDefault() ?? string.Empty;
            }
        }

        await GetAssessment(_language);
    }

    private async Task GetAssessment(Language language)
    {
        _assessment = await AssessmentService.GetAssessment(AssessmentId, language);
        StateHasChanged();
    }

    private async Task StartAssessment()
    {
        var userAssessmentId = await CreateUserAssessment();
        var userAssessmentTaskId = await UserAssessmentTaskService.GetNextUserAssessmentTaskId(userAssessmentId);
        NavigationManager.NavigateTo($"/user/assessments/{userAssessmentId}/tasks/{userAssessmentTaskId}");
    }

    private async Task AddToMyAssessments()
    {
        await CreateUserAssessment();
        NavigationManager.NavigateTo("/user/assessments");
    }

    private async Task<Guid> CreateUserAssessment()
    {
        return await UserAssessmentService.CreateUserAssessment(_userId, AssessmentId, _language);
    }

    private async Task LanguageChanged(Language language)
    {
        _language = language;
        await GetAssessment(_language);
    }
}
