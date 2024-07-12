using Microsoft.AspNetCore.Components;
using Therasim.Web.Components.Simulations;

namespace Therasim.Web.Components.Pages;

public partial class Simulations : ComponentBase
{
    private ListSimulations ListSimulationsComponent { get; set; } = null!;

    private async Task HandleSimulationCreated()
    {
        await ListSimulationsComponent.GetSimulations();
    }
}