using MediatR;
using Therasim.Application.Assessments.Commands.CreateAssessment;
using Therasim.Application.Assessments.Commands.EndAssessment;
using Therasim.Application.Assessments.Commands.SaveAssessmentChatHistory;
using Therasim.Application.Assessments.Commands.StartAssessment;
using Therasim.Application.Assessments.Queries.GetAssessment;
using Therasim.Application.Assessments.Queries.GetAssessments;
using Therasim.Domain.Enums;
using Therasim.Web.Models;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class AssessmentService : IAssessmentService
{
    private readonly IMediator _mediator;

    public AssessmentService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IQueryable<AssessmentDto>> GetAssessments(string userId)
    {
        var query = new GetAssessmentsQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return result.AsQueryable();
    }

    public async Task<AssessmentDetailsDto> GetAssessment(Guid assessmentId)
    {
        var query = new GetAssessmentQuery(assessmentId);
        return await _mediator.Send(query);
    }

    public async Task<Guid> CreateAssessment(CreateAssessmentModel createAssessmentModel)
    {
        var command = new CreateAssessmentCommand
        {
            UserId = createAssessmentModel.UserId,
            PersonaId = Guid.Parse(createAssessmentModel.PersonaId),
            SkillId = Guid.Parse(createAssessmentModel.SkillId),
            Language = Enum.Parse<Language>(createAssessmentModel.Language)
        };

        return await _mediator.Send(command);
    }

    public async Task<bool> SaveChatHistory(Guid assessmentId, string chatHistory)
    {
        var command = new SaveAssessmentChatHistoryCommand { AssessmentId = assessmentId, ChatHistory = chatHistory };
        return await _mediator.Send(command);
    }

    public async Task<bool> StartAssessment(Guid assessmentId)
    {
        var command = new StartAssessmentCommand { AssessmentId = assessmentId };
        return await _mediator.Send(command);
    }

    public async Task<bool> EndAssessment(Guid assessmentId)
    {
        var command = new EndAssessmentCommand { AssessmentId = assessmentId };
        return await _mediator.Send(command);
    }
}
