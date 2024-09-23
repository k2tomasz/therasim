using Therasim.Application.Assessments.Queries.GetAssessments;

namespace Therasim.Web.Services.Interfaces;

public interface IAssessmentService
{
    Task<IQueryable<AssessmentDto>> GetAssessments();
}
