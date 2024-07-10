using Therasim.Domain.Entities;

namespace Therasim.Application.Skills.Queries.GetSkills;

public class SkillDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Skill, SkillDto>();
        }
    }
}
