using MediatR;
using Therasim.Application.Simulations.Commands.CreateSimulation;
using Therasim.Application.Simulations.Queries.GetSimulations;
using Therasim.Domain.Enums;
using Therasim.Web.Models;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class SimulationService(IMediator mediator) : ISimulationService
{
    public async Task<IQueryable<SimulationDto>> GetSimulations(string userId)
    {
        var query = new GetSimulationsQuery { UserId = userId };
        var result = await mediator.Send(query);

        return result.AsQueryable();
    }

    public async Task<Guid> CreateSimulation(CreateSimulationModel model)
    {
        var command = new CreateSimulationCommand
        {
            UserId = model.UserId,
            PersonaId = Guid.Parse(model.PersonaId), 
            SkillId = Guid.Parse(model.SkillId),
            ProblemId = Guid.Parse(model.ProblemId),
            Language = Enum.Parse<Language>(model.Language),
            FeedbackType = Enum.Parse<FeedbackType>(model.FeedbackType)
        };

        return await mediator.Send(command);
    }

    
}
