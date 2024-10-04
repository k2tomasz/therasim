using Therasim.Domain.Entities;

namespace Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTasks;

public class UserAssessmentTaskDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Scenario { get; set; } = null!;
    public string Challenge { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserAssessmentTask, UserAssessmentTaskDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.AssessmentTask.AssessmentTaskLanguages.Where(x=>x.Language == s.Language).First().Name))
                .ForMember(d => d.Scenario, opt => opt.MapFrom(s => s.AssessmentTask.AssessmentTaskLanguages.Where(x => x.Language == s.Language).First().Scenario))
                .ForMember(d => d.Challenge, opt => opt.MapFrom(s => s.AssessmentTask.AssessmentTaskLanguages.Where(x => x.Language == s.Language).First().Challenge))
                .ForMember(d => d.StartDate, opt => opt.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, opt => opt.MapFrom(s => s.EndDate));
        }
    }
}
