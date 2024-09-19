using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Assessments.Queries.GetAssessments;

public class AssessmentDto
{
    public Guid Id { get; set; }
    public string Skill { get; set; } = string.Empty;
    public string Persona { get; set; } = string.Empty;
    public Language Language { get; set; }
    public bool Started { get; set; }
    public bool Completed { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Assessment, AssessmentDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Skill, opt => opt.MapFrom(s => s.Skill.Name))
                .ForMember(d => d.Persona, opt => opt.MapFrom(s => s.Persona.Name))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Language))
                .ForMember(d => d.Started, opt => opt.MapFrom(s => s.StartDate.HasValue))
                .ForMember(d => d.Completed, opt => opt.MapFrom(s => s.EndDate.HasValue));
        }
    }
}