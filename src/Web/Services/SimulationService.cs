using MediatR;
using Therasim.Application.Simulations.Commands.CreateSimulation;
using Therasim.Application.Simulations.Queries.GetSimulations;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class SimulationService : ISimulationService
{
    private readonly IMediator _mediator;

    public SimulationService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<IList<SimulationDto>> GetSimulations(string userId)
    {
        var query = new GetSimulationsQuery { UserId = userId };
        return _mediator.Send(query);
    }

    public Task<Guid> CreateSimulation(string userId, Guid personaId, Guid skillId, Guid psychProblemId)
    {
        var command = new CreateSimulationCommand
        {
            UserId = userId, PersonaId = personaId, SkillId = skillId, PsychProblemId = psychProblemId
        };
        return _mediator.Send(command);
    }

    
}
