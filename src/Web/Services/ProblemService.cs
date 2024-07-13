using MediatR;
using Therasim.Application.Problems.Queries.GetProblems;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class ProblemService : IProblemService
{
    private readonly IMediator _mediator;

    public ProblemService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IList<ProblemDto>> GetProblems()
    {
        return await _mediator.Send(new GetProblemsQuery());
    }
}
