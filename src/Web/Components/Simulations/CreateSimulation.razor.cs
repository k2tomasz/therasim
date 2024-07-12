using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Therasim.Application.Personas.Queries.GetPersonas;
using Therasim.Application.PsychProblems.Queries.GetPsychProblems;
using Therasim.Application.Skills.Queries.GetSkills;
using Therasim.Web.Models;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.Simulations;

public partial class CreateSimulation : ComponentBase
{
    [Inject] private IPersonaService PersonaService { get; set; } = null!;
    [Inject] private ISkillService SkillService { get; set; } = null!;
    [Inject] private IPsychProblemsService PsychProblemsService { get; set; } = null!;
    [Inject] private ISimulationService SimulationService { get; set; } = null!;
    [Parameter] public EventCallback OnSimulationCreated { get; set; }
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    public string? PersonaId { get; set; } = null;

    [SupplyParameterFromForm]
    private CreateSimulationModel CreateSimulationModel { get; set; } = new();
    //private ValidationMessageStore? messageStore;
    private IList<PersonaDto> _personas = new List<PersonaDto>();
    private IList<SkillDto> _skills = new List<SkillDto>();
    private IList<PsychProblemDto> _psychProblems = new List<PsychProblemDto>();

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

                CreateSimulationModel.UserId = userId;
            }
        }

        _personas = await PersonaService.GetPersonas();
        _skills = await SkillService.GetSkills();
        _psychProblems = await PsychProblemsService.GetPsychProblems();
    }

    private async Task HandleValidSubmitAsync()
    {
        await SimulationService.CreateSimulation(CreateSimulationModel);
        await OnSimulationCreated.InvokeAsync();
    }
}
