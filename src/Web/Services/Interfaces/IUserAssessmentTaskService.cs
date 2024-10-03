using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTasks;

namespace Therasim.Web.Services.Interfaces;

public interface IUserAssessmentTaskService
{
    Task<UserAssessmentTaskDetailsDto> GetUserAssessmentTask(Guid userAssessmentTaskId);
    Task<IList<UserAssessmentTaskDto>> GetUserAssessmentTasks(Guid userAssessmentId);
    Task<Guid> GetNextUserAssessmentTaskId(Guid userAssessmentId);
    Task SaveAssessmentTaskChatHistory(Guid userAssessmentTaskId, string chatHistory);
    Task StartAssessmentTask(Guid userAssessmentTaskId);
    Task EndAssessmentTask(Guid userAssessmentTaskId);
    Task<string> GenerateAssessmentTaskFeedback(Guid userAssessmentTaskId);
}
