using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.UserAssessments.Queries.GetUserAssessments;

public class UserAssessmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Language { get; set; } = null!;
    public Guid NextUserAssessmentTaskId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserAssessment, UserAssessmentDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Assessment.AssessmentLanguages.Where(x => x.Language == s.Language).First().Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Assessment.AssessmentLanguages.Where(x => x.Language == s.Language).First().Description))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Language.ToString()))
                .ForMember(d => d.NextUserAssessmentTaskId, opt => opt.MapFrom(s => s.UserAssessmentTasks.Any() ? s.UserAssessmentTasks.First().Id : Guid.Empty));
        }
    }
}