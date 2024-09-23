using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessmentTasks.Commands.StartUserAssessmentTask;

public record StartUserAssessmentTaskCommand(Guid UserAssessmentTaskId) : IRequest;

public class StartUserAssessmentTaskCommandValidator : AbstractValidator<StartUserAssessmentTaskCommand>
{
    public StartUserAssessmentTaskCommandValidator()
    {
    }
}

public class StartUserAssessmentTaskCommandHandler : IRequestHandler<StartUserAssessmentTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public StartUserAssessmentTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(StartUserAssessmentTaskCommand request, CancellationToken cancellationToken)
    {
        var userAssessmentTask = await _context.UserAssessmentTasks.FindAsync(request.UserAssessmentTaskId);

        if (userAssessmentTask == null) throw new NotFoundException(request.UserAssessmentTaskId.ToString(), "UserAssessmentTask");

        userAssessmentTask.StartDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
