using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;

namespace Therasim.Application.Simulations.Commands.DeleteSimulation;

public record DeleteSimulationCommand(Guid SimulationId) : IRequest<bool>;

public class DeleteSimulationCommandValidator : AbstractValidator<DeleteSimulationCommand>
{
    public DeleteSimulationCommandValidator()
    {
    }
}

public class DeleteSimulationCommandHandler : IRequestHandler<DeleteSimulationCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteSimulationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteSimulationCommand request, CancellationToken cancellationToken)
    {
        var simulation = await _context.Simulations.FindAsync(request.SimulationId);
        
        if (simulation != null)
        {
            _context.Simulations.Remove(simulation);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
