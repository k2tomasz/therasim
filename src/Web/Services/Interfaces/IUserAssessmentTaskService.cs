using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;

namespace Therasim.Web.Services.Interfaces;

public interface IUserAssessmentTaskService
{
    Task<UserAssessmentTaskDto> GetUserAssessmentTask(Guid userAssessmentTaskId);
    Task<Guid> GetNextUserAssessmentTaskId(Guid userAssessmentId);
    Task SaveAssessmentTaskChatHistory(Guid userAssessmentTaskId, string chatHistory);
    Task StartAssessmentTask(Guid userAssessmentTaskId);
    Task EndAssessmentTask(Guid userAssessmentTaskId);
    Task GenerateAssessmentTaskFeedback(Guid userAssessmentTaskId);
}
