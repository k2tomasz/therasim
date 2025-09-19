using MediatR;
using Therasim.Application.Assessments.Queries.GetAssessment;
using Therasim.Application.Assessments.Queries.GetAssessments;
using Therasim.Domain.Enums;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class AssessmentService : IAssessmentService
{
    private readonly IMediator _mediator;

    public AssessmentService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<AssessmentDetailsDto> GetAssessment(Guid assessmentId, Language language)
    {
        var query = new GetAssessmentQuery(assessmentId, language);
        return await _mediator.Send(query);
    }

    public Task<IList<AssessmentDto>> GetAssessments()
    {
        var query = new GetAssessmentsQuery();
        return _mediator.Send(query);
    }
}
