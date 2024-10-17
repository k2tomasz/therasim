using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.UserAssessments.Commands.CreateUserAssessment;

public record CreateUserAssessmentCommand : IRequest<Guid>
{
    public string UserId { get; init; } = null!;
    public Guid AssessmentId { get; init; }
    public Language Language { get; init; }
}

public class CreateAssessmentCommandValidator : AbstractValidator<CreateUserAssessmentCommand>
{
    public CreateAssessmentCommandValidator()
    {
    }
}

public class CreateUserAssessmentCommandHandler : IRequestHandler<CreateUserAssessmentCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateUserAssessmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUserAssessmentCommand request, CancellationToken cancellationToken)
    {
        var assessmentTasks = await _context.AssessmentTasks
            .Where(x => x.AssessmentId == request.AssessmentId)
            .ToArrayAsync(cancellationToken);

        var entity = new UserAssessment
        {
            UserId = request.UserId,
            AssessmentId = request.AssessmentId,
            Language = request.Language
        };

        foreach (var assessmentTask in assessmentTasks)
        {
            entity.UserAssessmentTasks.Add(new UserAssessmentTask
            {
                UserId = request.UserId,
                AssessmentTaskId = assessmentTask.Id,
                Language = request.Language,
                Order = assessmentTask.Order,
                UserAssessment = entity,
            });
        }

        _context.UserAssessments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
