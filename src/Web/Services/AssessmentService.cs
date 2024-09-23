using MediatR;
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

    public async Task<IQueryable<AssessmentDto>> GetAssessments()
    {
        var query = new GetAssessmentsQuery();
        var result = await _mediator.Send(query);
        return result.AsQueryable();
    }
}
