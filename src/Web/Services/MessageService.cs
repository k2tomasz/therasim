using MediatR;
using Therasim.Application.Messages.Commands.AddSessionMessage;
using Therasim.Application.Messages.Queries.GetSessionMessages;
using Therasim.Domain.Enums;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class MessageService(IMediator mediator) : IMessageService
{
    public async Task<Guid> AddSessionMessage(Guid sessionId, string message, MessageAuthorRole role)
    {
        var addSessionMessage = new AddSessionMessageCommand(sessionId, message, role);
        return await mediator.Send(addSessionMessage);
    }

    public async Task<IList<MessageDto>> GetSessionMessages(Guid sessionId)
    {
        var getSessionMessagesQuery = new GetSessionMessagesQuery(sessionId);
        return await mediator.Send(getSessionMessagesQuery);
    }
}
