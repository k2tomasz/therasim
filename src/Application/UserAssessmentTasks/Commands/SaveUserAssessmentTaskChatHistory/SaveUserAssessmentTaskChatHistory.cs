using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.UserAssessmentTasks.Commands.SaveUserAssessmentTaskChatHistory;

public record SaveUserAssessmentTaskChatHistoryCommand : IRequest<bool>
{
    public Guid UserAssessmentTaskId { get; init; }
    public string ChatHistory { get; init; } = string.Empty;
}

public class SaveUserAssessmentTaskChatHistoryCommandValidator : AbstractValidator<SaveUserAssessmentTaskChatHistoryCommand>
{
    public SaveUserAssessmentTaskChatHistoryCommandValidator()
    {
    }
}

public class SaveUserAssessmentTaskChatHistoryCommandHandler : IRequestHandler<SaveUserAssessmentTaskChatHistoryCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public SaveUserAssessmentTaskChatHistoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(SaveUserAssessmentTaskChatHistoryCommand request, CancellationToken cancellationToken)
    {
        var userAssessmentTask = await _context.UserAssessmentTasks.FindAsync(request.UserAssessmentTaskId);

        if (userAssessmentTask == null) throw new NotFoundException(request.UserAssessmentTaskId.ToString(), "UserAssessmentTask");

        userAssessmentTask.ChatHistory = request.ChatHistory;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
