using MediatR;
using Therasim.Application.UserAssessmentTasks.Commands.EndUserAssessmentTask;
using Therasim.Application.UserAssessmentTasks.Commands.GenerateUserAssessmentTaskFeedback;
using Therasim.Application.UserAssessmentTasks.Commands.SaveUserAssessmentTaskChatHistory;
using Therasim.Application.UserAssessmentTasks.Commands.StartUserAssessmentTask;
using Therasim.Application.UserAssessmentTasks.Queries.GetNextUserAssessmentTask;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class UserAssessmentTaskService : IUserAssessmentTaskService
{
    private readonly IMediator _mediator;

    public UserAssessmentTaskService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<UserAssessmentTaskDto> GetUserAssessmentTask(Guid userAssessmentTaskId)
    {
        var query = new GetUserAssessmentTaskQuery(userAssessmentTaskId);
        return await _mediator.Send(query);
    }

    public async Task<Guid> GetNextUserAssessmentTaskId(Guid userAssessmentId)
    {
        var query = new GetNextUserAssessmentTaskQuery(userAssessmentId);
        return await _mediator.Send(query);
    }


    public async Task SaveAssessmentTaskChatHistory(Guid userAssessmentTaskId, string chatHistory)
    {
        var command = new SaveUserAssessmentTaskChatHistoryCommand { UserAssessmentTaskId = userAssessmentTaskId, ChatHistory = chatHistory };
        await _mediator.Send(command);
    }

    public async Task StartAssessmentTask(Guid userAssessmentTaskId)
    {
        var command = new StartUserAssessmentTaskCommand(userAssessmentTaskId);
        await _mediator.Send(command);
    }

    public async Task EndAssessmentTask(Guid userAssessmentTaskId)
    {
        var command = new EndUserAssessmentTaskCommand(userAssessmentTaskId);
        await _mediator.Send(command);
    }

    public async Task GenerateAssessmentTaskFeedback(Guid userAssessmentTaskId)
    {
        var command = new GenerateUserAssessmentTaskFeedbackCommand(userAssessmentTaskId);
        await _mediator.Send(command);
    }
}
