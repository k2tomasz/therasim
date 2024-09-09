using Therasim.Application.Assessments.Queries.GetAssessment;
using Therasim.Application.Assessments.Queries.GetAssessments;
using Therasim.Web.Models;

namespace Therasim.Web.Services.Interfaces;

public interface IAssessmentService
{
    Task<IQueryable<AssessmentDto>> GetAssessments(string userId);
    Task<AssessmentDetailsDto> GetAssessment(Guid assessmentId);
    Task<Guid> CreateAssessment(CreateAssessmentModel createAssessmentModel);
    Task<bool> SaveChatHistory(Guid assessmentId, string chatHistory);
    Task<bool> StartAssessment(Guid assessmentId);
    Task<bool> EndAssessment(Guid assessmentId);
}
