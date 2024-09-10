using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Sessions.Commands.SaveSessionFeedbackHistory;

public record SaveSessionFeedbackHistoryCommand(Guid SessionId, string FeedbackHistory) : IRequest<bool>;

public class SaveSessionFeedbackHistoryCommandValidator : AbstractValidator<SaveSessionFeedbackHistoryCommand>
{
    public SaveSessionFeedbackHistoryCommandValidator()
    {
    }
}

public class SaveSessionFeedbackHistoryCommandHandler : IRequestHandler<SaveSessionFeedbackHistoryCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public SaveSessionFeedbackHistoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(SaveSessionFeedbackHistoryCommand request, CancellationToken cancellationToken)
    {
        var session = await _context.Sessions.FindAsync(request.SessionId);

        if (session == null) throw new NotFoundException(request.SessionId.ToString(), "Session");

        session.FeedbackHistory = request.FeedbackHistory;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
