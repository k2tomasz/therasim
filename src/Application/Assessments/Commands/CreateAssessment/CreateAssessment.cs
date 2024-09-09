using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Assessments.Commands.CreateAssessment;

public record CreateAssessmentCommand : IRequest<Guid>
{
    public string UserId { get; init; } = null!;
    public Guid PersonaId { get; init; }
    public Guid SkillId { get; init; }
    public Language Language { get; set; }
}

public class CreateAssessmentCommandValidator : AbstractValidator<CreateAssessmentCommand>
{
    public CreateAssessmentCommandValidator()
    {
    }
}

public class CreateAssessmentCommandHandler : IRequestHandler<CreateAssessmentCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateAssessmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAssessmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Assessment
        {
            UserId = request.UserId,
            PersonaId = request.PersonaId,
            SkillId = request.SkillId,
            Language = request.Language
        };

        _context.Assessments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
