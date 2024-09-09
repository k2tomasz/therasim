using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Assessments.Commands.StartAssessment;

public record StartAssessmentCommand : IRequest<bool>
{
    public Guid AssessmentId { get; set; }
}

public class StartAssessmentCommandValidator : AbstractValidator<StartAssessmentCommand>
{
    public StartAssessmentCommandValidator()
    {
    }
}

public class StartAssessmentCommandHandler : IRequestHandler<StartAssessmentCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public StartAssessmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(StartAssessmentCommand request, CancellationToken cancellationToken)
    {
        var assessment = await _context.Assessments.FindAsync(request.AssessmentId);
        if (assessment != null)
        {
            assessment.StartDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        else
        {
            return false;
        }
    }
}
