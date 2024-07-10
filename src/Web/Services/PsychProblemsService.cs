using MediatR;
using Therasim.Application.PsychProblems.Queries.GetPsychProblems;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class PsychProblemsService : IPsychProblemsService
{
    private readonly IMediator _mediator;

    public PsychProblemsService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IList<PsychProblemDto>> GetPsychProblems()
    {
        return await _mediator.Send(new GetPsychProblemsQuery());
    }
}
