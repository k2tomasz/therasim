using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Assessments.Commands.GenerateAssessmentFeedback;

public record GenerateAssessmentFeedbackCommand(Guid AssessmentId) : IRequest;
public class GenerateAssessmentFeedbackCommandValidator : AbstractValidator<GenerateAssessmentFeedbackCommand>
{
    public GenerateAssessmentFeedbackCommandValidator()
    {
    }
}

public class GenerateAssessmentFeedbackCommandHandler : IRequestHandler<GenerateAssessmentFeedbackCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILanguageModelService _languageModelService;

    public GenerateAssessmentFeedbackCommandHandler(IApplicationDbContext context, ILanguageModelService languageModelService)
    {
        _context = context;
        _languageModelService = languageModelService;
    }

    public async Task Handle(GenerateAssessmentFeedbackCommand request, CancellationToken cancellationToken)
    {
        var assessment = await _context.Assessments.FindAsync(request.AssessmentId);

        if (assessment == null) throw new NotFoundException(request.AssessmentId.ToString(), "Assessment");

        var feedback = await _languageModelService.GenerateAssessmentFeedback(assessment.ChatHistory);

        assessment.Feedback = feedback;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
