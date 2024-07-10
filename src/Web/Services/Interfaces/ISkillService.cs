using Therasim.Application.Skills.Queries.GetSkills;

namespace Therasim.Web.Services.Interfaces;

public interface ISkillService
{
    Task<IList<SkillDto>> GetSkills();
}
