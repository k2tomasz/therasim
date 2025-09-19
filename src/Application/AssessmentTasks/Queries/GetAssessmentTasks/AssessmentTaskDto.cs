using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.AssessmentTasks.Queries.GetAssessmentTasks;

public class AssessmentTaskDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Scenario { get; set; } = null!;
    public string Challenge { get; set; } = null!;
    public string Skills { get; set; } = null!;
    public Language Language { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AssessmentTask, AssessmentTaskDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.AssessmentTaskLanguages.First().Name))
                .ForMember(d => d.Scenario, opt => opt.MapFrom(s => s.AssessmentTaskLanguages.First().Scenario))
                .ForMember(d => d.Challenge, opt => opt.MapFrom(s => s.AssessmentTaskLanguages.First().Challenge))
                .ForMember(d => d.Skills, opt => opt.MapFrom(s => s.AssessmentTaskLanguages.First().Skills))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.AssessmentTaskLanguages.First().Language));
        }
    }
}