using MediatR;
using Therasim.Application.Skills.Queries.GetSkills;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Services;

public class SkillService(IMediator mediator) : ISkillService
{
    public async Task<IList<SkillDto>> GetSkills()
    {
        return await mediator.Send(new GetSkillsQuery());
    }
}
