using Therasim.Application.Feedbacks.Queries.GetSessionFeedbacks;

namespace Therasim.Web.Services.Interfaces;

public interface IFeedbackService
{
    Task<Guid> AddSessionFeedback(Guid sessionId, string content, string message);
    Task<IList<FeedbackDto>> GetSessionFeedbacks(Guid sessionId);
}