using MediatR;
using Therasim.Application.Assessments.Queries.GetAssessments;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class AssessmentService : IAssessmentService
{
    private readonly IMediator _mediator;

    public AssessmentService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IList<AssessmentDto>> GetAssessments()
    {
        var query = new GetAssessmentsQuery();
        return await _mediator.Send(query);
    }
}
