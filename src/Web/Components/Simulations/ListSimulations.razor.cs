using Microsoft.AspNetCore.Components;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Simulations.Queries.GetSimulations;

namespace Therasim.Web.Components.Simulations;

public partial class ListSimulations : ComponentBase
{
    [Inject] private ISimulationService SimulationService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private IQueryable<SimulationDto> Simulations { get; set; } = new List<SimulationDto>().AsQueryable();

    protected override async Task OnInitializedAsync()
    {
        Simulations = (await SimulationService.GetSimulations("userId")).AsQueryable();
    }

    private Task StartSimulation(SimulationDto context)
    {
        NavigationManager.NavigateTo("/chat");
        return Task.CompletedTask;
    }
}
