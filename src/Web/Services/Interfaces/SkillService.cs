using MediatR;
using Therasim.Application.Skills.Queries.GetSkills;

namespace Therasim.Web.Services.Interfaces;

public class SkillService : ISkillService
{
    private readonly IMediator _mediator;

    public SkillService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IList<SkillDto>> GetSkills()
    {
        return await _mediator.Send(new GetSkillsQuery());
    }
}
