using Therasim.Application.UserAssessments.Queries.GetUserAssessment;
using Therasim.Application.UserAssessments.Queries.GetUserAssessments;
namespace Therasim.Web.Services.Interfaces;

public interface IUserAssessmentService
{
    Task<IQueryable<UserAssessmentDto>> GetUserAssessments(string userId);
    Task<UserAssessmentDetailsDto> GetUserAssessment(Guid userAssessmentId);
    Task<Guid> CreateUserAssessment(string userId, Guid assessmentId);
    Task GenerateUserAssessmentFeedback(Guid userAssessmentId);
}
