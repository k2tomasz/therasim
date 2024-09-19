using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;
namespace Therasim.Application.UserAssessments.Commands.CreateUserAssessment;

public record CreateUserAssessmentCommand : IRequest<Guid>
{
    public string UserId { get; init; } = null!;
    public Guid AssessmentId { get; init; }
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
        var entity = new UserAssessment
        {
            UserId = request.UserId,
            AssessmentId = request.AssessmentId,
        };

        _context.UserAssessments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
