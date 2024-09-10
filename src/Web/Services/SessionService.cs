using MediatR;
using Therasim.Application.Sessions.Commands.CreateSession;
using Therasim.Application.Sessions.Commands.SaveSessionChatHistory;
using Therasim.Application.Sessions.Commands.SaveSessionFeedbackHistory;
using Therasim.Application.Sessions.Queries.GetSession;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class SessionService(IMediator mediator) : ISessionService
{
    public async Task<Guid> CreateSession(Guid simulationId, bool isActive = true)
    {
        var createSessionCommand = new CreateSessionCommand(simulationId, isActive);
        return await mediator.Send(createSessionCommand);
    }

    public async Task<SessionDto> GetSession(Guid sessionId)
    {
        var getSessionQuery = new GetSessionQuery(sessionId);
        return await mediator.Send(getSessionQuery);
    }

    public async Task<bool> SaveChatHistory(Guid sessionId, string chatHistory)
    {
        var saveChatHistoryCommand = new SaveSessionChatHistoryCommand(sessionId, chatHistory);
        return await mediator.Send(saveChatHistoryCommand);
    }

    public async Task<bool> SaveFeedbackHistory(Guid sessionId, string feedbackHistory)
    {
        var saveFeedbackHistoryCommand = new SaveSessionFeedbackHistoryCommand(sessionId, feedbackHistory);
        return await mediator.Send(saveFeedbackHistoryCommand);
    }
}