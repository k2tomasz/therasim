using MediatR;
using Therasim.Application.Feedbacks.Commands.AddSessionFeedback;
using Therasim.Application.Feedbacks.Queries.GetSessionFeedbacks;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class FeedbackService(IMediator mediator) : IFeedbackService
{
    public async Task<Guid> AddSessionFeedback(Guid sessionId, string content, string message)
    {
        var addSessionFeedback = new AddSessionFeedbackCommand(sessionId, content, message);
        return await mediator.Send(addSessionFeedback);
    }

    public async Task<IList<FeedbackDto>> GetSessionFeedbacks(Guid sessionId)
    {
        var getSessionFeedbacks = new GetSessionFeedbacksQuery(sessionId);
        return await mediator.Send(getSessionFeedbacks);
    }
}