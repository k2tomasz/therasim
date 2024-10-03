using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessmentTasks.Commands.GenerateUserAssessmentTaskFeedback;

public record GenerateUserAssessmentTaskFeedbackCommand(Guid UserAssessmentTaskId) : IRequest<string>;

public class GenerateUserAssessmentTaskFeedbackCommandValidator : AbstractValidator<GenerateUserAssessmentTaskFeedbackCommand>
{
    public GenerateUserAssessmentTaskFeedbackCommandValidator()
    {
    }
}

public class GenerateUserAssessmentTaskFeedbackCommandHandler : IRequestHandler<GenerateUserAssessmentTaskFeedbackCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ILanguageModelService _languageModelService;

    public GenerateUserAssessmentTaskFeedbackCommandHandler(IApplicationDbContext context, ILanguageModelService languageModelService)
    {
        _context = context;
        _languageModelService = languageModelService;
    }

    public async Task<string> Handle(GenerateUserAssessmentTaskFeedbackCommand request, CancellationToken cancellationToken)
    {
        var userAssessmentTask = await _context.UserAssessmentTasks
            .Include(x => x.AssessmentTask.AssessmentTaskLanguages.Where(l=>l.Language == x.Language))
            .Where(x => x.Id == request.UserAssessmentTaskId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userAssessmentTask == null) throw new NotFoundException(request.UserAssessmentTaskId.ToString(), "UserAssessmentTask");

        var feedback = await _languageModelService.GenerateAssessmentFeedback(userAssessmentTask.ChatHistory, userAssessmentTask.AssessmentTask.AssessmentTaskLanguages.First().FeedbackSystemPrompt);

        userAssessmentTask.Feedback = feedback;
        await _context.SaveChangesAsync(cancellationToken);

        return feedback;
    }
}
