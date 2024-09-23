using Therasim.Application.UserAssessments.Queries.GetUserAssessment;
using Therasim.Application.UserAssessments.Queries.GetUserAssessments;
using Therasim.Web.Models;
namespace Therasim.Web.Services.Interfaces;

public interface IUserAssessmentService
{
    Task<IQueryable<UserAssessmentDto>> GetUserAssessments(string userId);
    Task<UserAssessmentDetailsDto> GetUserAssessment(Guid userAssessmentId);
    Task<Guid> CreateUserAssessment(CreateUserAssessmentModel createUserAssessmentModel);
    Task GenerateUserAssessmentFeedback(Guid userAssessmentId);
}
