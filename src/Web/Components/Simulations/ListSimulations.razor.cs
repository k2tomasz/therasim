using Microsoft.AspNetCore.Components;
using Therasim.Web.Services.Interfaces;
using Therasim.Application.Simulations.Queries.GetSimulations;
using Microsoft.AspNetCore.Components.Authorization;

namespace Therasim.Web.Components.Simulations;

public partial class ListSimulations : ComponentBase
{
    [Inject] private ISimulationService SimulationService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }
    private IQueryable<SimulationDto> Simulations { get; set; } = new List<SimulationDto>().AsQueryable();
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
            await GetSimulations();
        }
    }

    private void StartSimulation(SimulationDto simulation)
    {
        NavigationManager.NavigateTo($"/simulation/{simulation.Id}");
    }

    public async Task GetSimulations()
    {
        Simulations = await SimulationService.GetSimulations(UserId);
        StateHasChanged();
    }
}
