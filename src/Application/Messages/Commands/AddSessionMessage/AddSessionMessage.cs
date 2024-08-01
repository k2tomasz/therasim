using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Messages.Commands.AddSessionMessage;

public record AddSessionMessageCommand(Guid SessionId, string Content, MessageAuthorRole Role) : IRequest<Guid>;

public class AddSessionMessageCommandValidator : AbstractValidator<AddSessionMessageCommand>
{
    public AddSessionMessageCommandValidator()
    {
    }
}

public class AddSessionMessageCommandHandler : IRequestHandler<AddSessionMessageCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public AddSessionMessageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(AddSessionMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            Content = request.Content,
            Role = request.Role,
            SessionId = request.SessionId
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        return message.Id;
    }
}
