using Therasim.Application.UserAssessments.Queries.GetCompletedAssessments;
using Therasim.Application.UserAssessments.Queries.GetUserAssessment;
using Therasim.Application.UserAssessments.Queries.GetUserAssessments;
using Therasim.Domain.Enums;
namespace Therasim.Web.Services.Interfaces;

public interface IUserAssessmentService
{
    Task<IQueryable<UserAssessmentDto>> GetUserAssessments(string userId);
    Task<UserAssessmentDetailsDto> GetUserAssessment(Guid userAssessmentId);
    Task<Guid> CreateUserAssessment(string userId, Guid assessmentId, Language language);
    Task GenerateUserAssessmentFeedback(Guid userAssessmentId);
    Task<IQueryable<CompletedAssessmentDto>> GetCompletedAssessments();
}
