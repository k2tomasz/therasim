using Therasim.Application.Common.Interfaces;

namespace Therasim.Application.Sessions.Commands.SaveSessionChatHistory;

public record SaveSessionChatHistoryCommand(Guid SessionId, string ChatHistory) : IRequest<bool>;

public class SaveSessionChatHistoryCommandValidator : AbstractValidator<SaveSessionChatHistoryCommand>
{
    public SaveSessionChatHistoryCommandValidator()
    {
    }
}

public class SaveSessionChatHistoryCommandHandler : IRequestHandler<SaveSessionChatHistoryCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public SaveSessionChatHistoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(SaveSessionChatHistoryCommand request, CancellationToken cancellationToken)
    {
        var session = await _context.Sessions.FindAsync(request.SessionId);

        if (session == null) throw new NotFoundException(request.SessionId.ToString(), "Session");

        session.ChatHistory = request.ChatHistory;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
