using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Simulations.Commands.CreateSimulation;

public record CreateSimulationCommand : IRequest<Guid>
{
    public string UserId { get; init; } = null!;
    public Guid PersonaId { get; init; }
    public Guid ProblemId { get; set; }
    public Guid SkillId { get; init; }
    public FeedbackType FeedbackType { get; set; }
    public Language Language { get; set; }
}

public class CreateAssessmentCommandValidator : AbstractValidator<CreateSimulationCommand>
{
    public CreateAssessmentCommandValidator()
    {
    }
}

public class CreateSimulationCommandHandler : IRequestHandler<CreateSimulationCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSimulationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateSimulationCommand request, CancellationToken cancellationToken)
    {
        var entity = new Simulation
        {
            UserId = request.UserId,
            PersonaId = request.PersonaId,
            SkillId = request.SkillId,
            ProblemId = request.ProblemId,
            FeedbackType = request.FeedbackType,
            Language = request.Language
        };

        _context.Simulations.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
