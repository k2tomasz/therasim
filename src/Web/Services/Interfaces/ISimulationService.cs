using Therasim.Application.Simulations.Queries.GetSimulations;
using Therasim.Web.Models;

namespace Therasim.Web.Services.Interfaces;

public interface ISimulationService
{
    Task<IQueryable<SimulationDto>> GetSimulations(string userId);
    Task<Guid> CreateSimulation(CreateSimulationModel createSimulationModel);
}
