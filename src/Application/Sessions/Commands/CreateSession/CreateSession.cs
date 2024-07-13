using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;

namespace Therasim.Application.Sessions.Commands.CreateSession;

public record CreateSessionCommand : IRequest<Guid>
{
    public bool IsActive { get; set; } = true;
    public Guid SimulationId { get; set; }
}

public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionCommandValidator()
    {
    }
}

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = new Session
        {
            IsActive = request.IsActive,
            SimulationId = request.SimulationId
        };

        if (session.IsActive)
        {
            var currentActive = await _context.Sessions.FirstOrDefaultAsync(x => x.IsActive && x.SimulationId == request.SimulationId, cancellationToken: cancellationToken);
            if (currentActive != null)
            {
                currentActive.IsActive = false;
            }
        }

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync(cancellationToken);

        return session.Id;
    }
}
