using Therasim.Domain.Entities;

namespace Therasim.Application.UserAssessments.Queries.GetUserAssessment;

public class UserAssessmentDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Language { get; set; } = null!;
    public string? Feedback { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserAssessment, UserAssessmentDetailsDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Feedback, opt => opt.MapFrom(s => s.Feedback))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Assessment.Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Assessment.Description))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Assessment.Language.ToString()));
        }
    }
}