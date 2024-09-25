using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Assessments.Queries.GetAssessments;
public class AssessmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Languages { get; set; } = null!;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Assessment, AssessmentDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.AssessmentLanguages.First(x=>x.Language == Language.English).Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.AssessmentLanguages.First(x => x.Language == Language.English).Description))
                .ForMember(d => d.Languages, opt => opt.MapFrom(s => string.Join(", ", s.AssessmentLanguages.Select(x => x.Language.ToString()))));
        }
    }
}
