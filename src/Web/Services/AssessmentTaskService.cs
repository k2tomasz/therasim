using MediatR;
using Therasim.Application.AssessmentTasks.Queries.GetAssessmentTasks;
using Therasim.Domain.Enums;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class AssessmentTaskService : IAssessmentTaskService
{
    private readonly IMediator _mediator;

    public AssessmentTaskService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IList<AssessmentTaskDto>> GetAssessmentTasks(Guid assessmentId, Language language)
    {
        var query = new GetAssessmentTasksQuery(assessmentId, language);
        return await _mediator.Send(query);
    }
}
