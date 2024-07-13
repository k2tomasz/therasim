using MediatR;
using Therasim.Application.Sessions.Commands.CreateSession;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class SessionService(IMediator mediator) : ISessionService
{
    public async Task<Guid> CreateSession(Guid simulationId, bool isActive = true)
    {
        var createSessionCommand = new CreateSessionCommand(simulationId, isActive);
        return await mediator.Send(createSessionCommand);
    }
}