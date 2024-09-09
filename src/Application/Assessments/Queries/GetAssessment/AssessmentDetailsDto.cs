using Therasim.Domain.Entities;

namespace Therasim.Application.Assessments.Queries.GetAssessment;

public class AssessmentDetailsDto
{
    public Guid Id { get; set; }
    public string Skill { get; set; } = string.Empty;
    public string SkillDescription { get; set; } = string.Empty;
    public string? ChatHistory { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Assessment, AssessmentDetailsDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Skill, opt => opt.MapFrom(s => s.Skill.Name))
                .ForMember(d => d.SkillDescription, opt => opt.MapFrom(s => s.Skill.Description))
                .ForMember(d => d.ChatHistory, opt => opt.MapFrom(s => s.ChatHistory));
        }
    }
}