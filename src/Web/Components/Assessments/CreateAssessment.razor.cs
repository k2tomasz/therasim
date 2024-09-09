using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Therasim.Application.Personas.Queries.GetPersonas;
using Therasim.Application.Skills.Queries.GetSkills;
using Therasim.Web.Models;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.Assessments;

public partial class CreateAssessment : ComponentBase
{
    [Inject] private IPersonaService PersonaService { get; set; } = null!;
    [Inject] private ISkillService SkillService { get; set; } = null!;
    [Inject] private IAssessmentService AssessmentService { get; set; } = null!;
    [Parameter] public EventCallback OnAssessmentCreated { get; set; }
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    [SupplyParameterFromForm] private CreateAssessmentModel CreateAssessmentModel { get; set; } = new();
    private IList<PersonaDto> _personas = new List<PersonaDto>();
    private IList<SkillDto> _skills = new List<SkillDto>();

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

                CreateAssessmentModel.UserId = userId;
            }
        }

        _personas = await PersonaService.GetPersonas();
        _skills = await SkillService.GetSkills();
    }

    private async Task HandleValidSubmitAsync()
    {
        await AssessmentService.CreateAssessment(CreateAssessmentModel);
        await OnAssessmentCreated.InvokeAsync();
    }
}
