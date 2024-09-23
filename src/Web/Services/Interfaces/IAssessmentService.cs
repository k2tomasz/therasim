using Therasim.Application.Assessments.Queries.GetAssessments;

namespace Therasim.Web.Services.Interfaces;

public interface IAssessmentService
{
    Task<IList<AssessmentDto>> GetAssessments();
}
