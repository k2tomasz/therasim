using Microsoft.AspNetCore.Components;
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
    [Parameter] public EventCallback OnAssessmentCreated { get; set; }

    private CreateSimulationModel CreateSimulationModel { get; set; } = new CreateSimulationModel();

    // Sample data for dropdowns
    public IList<PersonaDto> Personas { get; set; } = new List<PersonaDto>();
    public IList<SkillDto> Skills { get; set; } = new List<SkillDto>();
    public IList<PsychProblemDto> PsychProblems { get; set; } = new List<PsychProblemDto>();

    protected override async Task OnInitializedAsync()
    {
        Personas = await PersonaService.GetPersonas();
        Skills = await SkillService.GetSkills();
        PsychProblems = await PsychProblemsService.GetPsychProblems();
    }

    private async Task HandleValidSubmitAsync()
    {
        await OnAssessmentCreated.InvokeAsync(null);
    }
}
