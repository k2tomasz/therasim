using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessments.Commands.GenerateUserAssessmentFeedback;

public record GenerateUserAssessmentFeedbackCommand(Guid UserAssessmentId) : IRequest;
public class GenerateUserAssessmentFeedbackCommandValidator : AbstractValidator<GenerateUserAssessmentFeedbackCommand>
{
    public GenerateUserAssessmentFeedbackCommandValidator()
    {
    }
}

public class GenerateUserAssessmentFeedbackCommandHandler : IRequestHandler<GenerateUserAssessmentFeedbackCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILanguageModelService _languageModelService;

    public GenerateUserAssessmentFeedbackCommandHandler(IApplicationDbContext context, ILanguageModelService languageModelService)
    {
        _context = context;
        _languageModelService = languageModelService;
    }

    public async Task Handle(GenerateUserAssessmentFeedbackCommand request, CancellationToken cancellationToken)
    {
        var userAssessment = await _context.UserAssessments
            .Include(x => x.Assessment.AssessmentLanguages.Where(l => l.Language == x.Language))
            .Where(x => x.Id == request.UserAssessmentId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userAssessment == null) throw new NotFoundException(request.UserAssessmentId.ToString(), "UserAssessment");

        var transcript = string.Empty;

        var feedback = await _languageModelService.GenerateAssessmentFeedback(transcript, userAssessment.Assessment.AssessmentLanguages.First().FeedbackSystemPrompt);

        userAssessment.Feedback = feedback;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
