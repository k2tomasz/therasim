using MediatR;
using Therasim.Application.UserAssessments.Commands.CreateUserAssessment;
using Therasim.Application.UserAssessments.Commands.GenerateUserAssessmentFeedback;
using Therasim.Application.UserAssessments.Queries.GetUserAssessment;
using Therasim.Application.UserAssessments.Queries.GetUserAssessments;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class UserAssessmentService : IUserAssessmentService
{
    private readonly IMediator _mediator;

    public UserAssessmentService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IQueryable<UserAssessmentDto>> GetUserAssessments(string userId)
    {
        var query = new GetUserAssessmentsQuery(userId);
        var result = await _mediator.Send(query);
        return result.AsQueryable();
    }

    public async Task<UserAssessmentDetailsDto> GetUserAssessment(Guid userAssessmentId)
    {
        var query = new GetUserAssessmentQuery(userAssessmentId);
        return await _mediator.Send(query);
    }

    public async Task<Guid> CreateUserAssessment(string userId, Guid assessmentId)
    {
        var command = new CreateUserAssessmentCommand
        {
            UserId = userId,
            AssessmentId = assessmentId,
        };

        return await _mediator.Send(command);
    }

    public async Task GenerateUserAssessmentFeedback(Guid userAssessmentId)
    {
        var command = new GenerateUserAssessmentFeedbackCommand(userAssessmentId);
        await _mediator.Send(command);
    }
}
