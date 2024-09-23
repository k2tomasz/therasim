using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Therasim.Application.Assessments.Queries.GetAssessments;
using Therasim.Web.Models;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.Assessments;

public partial class CreateUserAssessment : ComponentBase
{
    [Inject] private IAssessmentService AssessmentService { get; set; } = null!;
    [Inject] private IUserAssessmentService UserAssessmentService { get; set; } = null!;
    [Parameter] public EventCallback OnAssessmentCreated { get; set; }
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    [SupplyParameterFromForm] private CreateUserAssessmentModel CreateUserAssessmentModel { get; set; } = new();
    private IList<AssessmentDto> _availableAssessments = new List<AssessmentDto>();

    protected override async Task OnInitializedAsync()
    {
        if (AuthenticationState is not null)
        {
            var state = await AuthenticationState;
            var principal = state.User;
            if (principal.Identity?.IsAuthenticated == true)
            {
                var userId = state.User.Claims
                     .Where(c => c.Type.Equals(@"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
                     .Select(c => c.Value)
                     .FirstOrDefault() ?? string.Empty;

                CreateUserAssessmentModel.UserId = userId;
            }
        }

        _availableAssessments = await AssessmentService.GetAssessments();
    }

    private async Task HandleValidSubmitAsync()
    {
        await UserAssessmentService.CreateUserAssessment(CreateUserAssessmentModel);
        await OnAssessmentCreated.InvokeAsync();
    }
}
