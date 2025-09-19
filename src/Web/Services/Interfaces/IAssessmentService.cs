using Therasim.Application.Assessments.Queries.GetAssessment;
using Therasim.Application.Assessments.Queries.GetAssessments;
using Therasim.Domain.Enums;

namespace Therasim.Web.Services.Interfaces;

public interface IAssessmentService
{
    Task<AssessmentDetailsDto> GetAssessment(Guid assessmentId, Language language);
    Task<IList<AssessmentDto>> GetAssessments();
}
