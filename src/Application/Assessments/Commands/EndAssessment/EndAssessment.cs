using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Assessments.Commands.EndAssessment;

public record EndAssessmentCommand : IRequest<bool>
{
    public Guid AssessmentId { get; set; }
}

public class EndAssessmentCommandValidator : AbstractValidator<EndAssessmentCommand>
{
    public EndAssessmentCommandValidator()
    {
    }
}

public class EndAssessmentCommandHandler : IRequestHandler<EndAssessmentCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public EndAssessmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(EndAssessmentCommand request, CancellationToken cancellationToken)
    {
        var assessment = await _context.Assessments.FindAsync(request.AssessmentId);

        if (assessment == null) throw new NotFoundException(request.AssessmentId.ToString(), "Assessment");
        
        assessment.EndDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
