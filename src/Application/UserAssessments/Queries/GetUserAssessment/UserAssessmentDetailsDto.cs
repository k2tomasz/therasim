using Therasim.Domain.Entities;

namespace Therasim.Application.UserAssessments.Queries.GetUserAssessment;

public class UserAssessmentDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public string Language { get; set; } = null!;
    public string? Feedback { get; set; }
    public Guid NextUserAssessmentTaskId { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserAssessment, UserAssessmentDetailsDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Feedback, opt => opt.MapFrom(s => s.Feedback))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Assessment.AssessmentLanguages.First().Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Assessment.AssessmentLanguages.First().Description))
                .ForMember(d => d.Instructions, opt => opt.MapFrom(s => s.Assessment.AssessmentLanguages.First().Instructions))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Assessment.AssessmentLanguages.First().Language.ToString()))
                .ForMember(d => d.NextUserAssessmentTaskId, opt => opt.MapFrom(s => s.UserAssessmentTasks.First().Id));
        }
    }
}