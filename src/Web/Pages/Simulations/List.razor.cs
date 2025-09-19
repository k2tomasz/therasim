using Microsoft.AspNetCore.Components;
using Therasim.Web.Components.Simulations;

namespace Therasim.Web.Pages.Simulations;

public partial class List : ComponentBase
{
    private ListSimulations ListSimulationsComponent { get; set; } = null!;

    private async Task HandleSimulationCreated()
    {
        await ListSimulationsComponent.GetSimulations();
    }
}