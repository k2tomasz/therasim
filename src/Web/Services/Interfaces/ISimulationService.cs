using Therasim.Application.Simulations.Queries.GetSimulations;

namespace Therasim.Web.Services.Interfaces;

public interface ISimulationService
{
    Task<IList<SimulationDto>> GetSimulations(string userId);
    Task<Guid> CreateSimulation(string userId, Guid personaId, Guid skillId, Guid psychProblemId);
}
