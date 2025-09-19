using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessmentTasks.Commands.EndUserAssessmentTask;

public record EndUserAssessmentTaskCommand(Guid UserAssessmentTaskId) : IRequest;

public class EndUserAssessmentTaskCommandValidator : AbstractValidator<EndUserAssessmentTaskCommand>
{
    public EndUserAssessmentTaskCommandValidator()
    {
    }
}

public class EndUserAssessmentTaskCommandHandler : IRequestHandler<EndUserAssessmentTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public EndUserAssessmentTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(EndUserAssessmentTaskCommand request, CancellationToken cancellationToken)
    {
        var userAssessmentTask = await _context.UserAssessmentTasks.FindAsync(request.UserAssessmentTaskId);

        if (userAssessmentTask == null) throw new NotFoundException(request.UserAssessmentTaskId.ToString(), "UserAssessmentTask");

        userAssessmentTask.EndDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }
}