using Therasim.Application.Common.Interfaces;
using Therasim.Application.UserAssessmentTasks.Commands.GenerateUserAssessmentTaskFeedback;

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
    private readonly IMediator _mediator;

    public GenerateUserAssessmentFeedbackCommandHandler(IApplicationDbContext context, ILanguageModelService languageModelService, IMediator mediator)
    {
        _context = context;
        _languageModelService = languageModelService;
        _mediator = mediator;
    }

    public async Task Handle(GenerateUserAssessmentFeedbackCommand request, CancellationToken cancellationToken)
    {

        var userAssessmentTasks = await _context.UserAssessmentTasks
            .Include(uat => uat.AssessmentTask.AssessmentTaskLanguages)
            .Where(uat => uat.UserAssessmentId == request.UserAssessmentId && uat.Order > 0)
            .ToListAsync(cancellationToken);

        foreach (var userAssessmentTask in userAssessmentTasks)
        {
            if (!string.IsNullOrEmpty(userAssessmentTask.Feedback)) continue;
            var command = new GenerateUserAssessmentTaskFeedbackCommand(userAssessmentTask.Id);
            await _mediator.Send(command);
        }
    }
}
