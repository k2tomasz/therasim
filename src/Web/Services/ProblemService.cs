using MediatR;
using Therasim.Application.Problems.Queries.GetProblems;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class ProblemService(IMediator mediator) : IProblemService
{
    public async Task<IList<ProblemDto>> GetProblems()
    {
        return await mediator.Send(new GetProblemsQuery());
    }
}
