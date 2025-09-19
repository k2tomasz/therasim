using Therasim.Application.Sessions.Queries.GetSession;

namespace Therasim.Web.Services.Interfaces;

public interface ISessionService
{
    Task<Guid> CreateSession(Guid simulationId, bool isActive = true);
    Task<SessionDto> GetSession(Guid sessionId);
    Task<bool> SaveChatHistory(Guid sessionId, string chatHistory);
    Task<bool> SaveFeedbackHistory(Guid sessionId, string feedbackHistory);
}