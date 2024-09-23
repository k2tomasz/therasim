using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Assessments.Queries.GetAssessments;

public class AssessmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Language { get; set; } = null!;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserAssessment, AssessmentDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Assessment.Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Assessment.Description))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Assessment.Language.ToString()));
        }
    }
}