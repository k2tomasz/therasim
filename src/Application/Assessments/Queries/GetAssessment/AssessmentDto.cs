
using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Assessments.Queries.GetAssessment;
public class AssessmentDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public Language Language { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Assessment, AssessmentDetailsDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.AssessmentLanguages.First().Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.AssessmentLanguages.First().Description))
                .ForMember(d => d.Instructions, opt => opt.MapFrom(s => s.AssessmentLanguages.First().Instructions))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.AssessmentLanguages.First().Language));
        }
    }
}
