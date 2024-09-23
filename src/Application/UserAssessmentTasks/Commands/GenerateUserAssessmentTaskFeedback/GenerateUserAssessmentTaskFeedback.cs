using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessmentTasks.Commands.GenerateUserAssessmentTaskFeedback;

public record GenerateUserAssessmentTaskFeedbackCommand : IRequest
{
    public Guid UserAssessmentTaskId { get; init; }
}

public class GenerateUserAssessmentTaskFeedbackCommandValidator : AbstractValidator<GenerateUserAssessmentTaskFeedbackCommand>
{
    public GenerateUserAssessmentTaskFeedbackCommandValidator()
    {
    }
}

public class GenerateUserAssessmentTaskFeedbackCommandHandler : IRequestHandler<GenerateUserAssessmentTaskFeedbackCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILanguageModelService _languageModelService;

    public GenerateUserAssessmentTaskFeedbackCommandHandler(IApplicationDbContext context, ILanguageModelService languageModelService)
    {
        _context = context;
        _languageModelService = languageModelService;
    }

    public async Task Handle(GenerateUserAssessmentTaskFeedbackCommand request, CancellationToken cancellationToken)
    {
        var userAssessmentTask = await _context.UserAssessmentTasks
            .Include(x => x.AssessmentTask)
            .Where(x => x.Id == request.UserAssessmentTaskId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userAssessmentTask == null) throw new NotFoundException(request.UserAssessmentTaskId.ToString(), "UserAssessmentTask");

        var feedback = await _languageModelService.GenerateAssessmentFeedback(userAssessmentTask.ChatHistory, userAssessmentTask.AssessmentTask.FeedbackSystemPrompt);

        userAssessmentTask.Feedback = feedback;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
