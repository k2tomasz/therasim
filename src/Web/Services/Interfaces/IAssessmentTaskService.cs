using Therasim.Application.AssessmentTasks.Queries.GetAssessmentTasks;
using Therasim.Domain.Enums;

namespace Therasim.Web.Services.Interfaces;

public interface IAssessmentTaskService
{
    Task<IList<AssessmentTaskDto>> GetAssessmentTasks(Guid assessmentId, Language language);
}
