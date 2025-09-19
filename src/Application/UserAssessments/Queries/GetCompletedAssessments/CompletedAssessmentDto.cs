using Therasim.Domain.Entities;

namespace Therasim.Application.UserAssessments.Queries.GetCompletedAssessments;

public class CompletedAssessmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Language { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserId { get; set; } = null!;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserAssessment, CompletedAssessmentDto>()

                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.UserId))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Language.ToString()))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Assessment.AssessmentLanguages.Where(x => x.Language == s.Language).First().Name));
        }
    }
}