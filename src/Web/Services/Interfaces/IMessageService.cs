using Therasim.Application.Messages.Queries.GetSessionMessages;
using Therasim.Domain.Enums;

namespace Therasim.Web.Services.Interfaces;

public interface IMessageService
{
    Task<Guid> AddSessionMessage(Guid sessionId, string message, AuthorRole role);
    Task<IList<MessageDto>> GetSessionMessages(Guid sessionId);

}