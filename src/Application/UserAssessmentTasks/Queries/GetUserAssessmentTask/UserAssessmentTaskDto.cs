using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;

public class UserAssessmentTaskDto
{
    public Guid Id { get; set; }
    public string Objective { get; set; } = null!;
    public string ClientSystemPrompt { get; set; } = null!;
    public string FeedbackSystemPrompt { get; set; } = null!;
    public Language Language { get; set; }
    public int? LengthInMinutes { get; set; }
    public int? LengthInInteractionCycles { get; set; }
    public Guid AssessmentId { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserAssessmentTask, UserAssessmentTaskDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Objective, opt => opt.MapFrom(s => s.AssessmentTask.Objective))
                .ForMember(d => d.ClientSystemPrompt, opt => opt.MapFrom(s => s.AssessmentTask.ClientSystemPrompt))
                .ForMember(d => d.FeedbackSystemPrompt, opt => opt.MapFrom(s => s.AssessmentTask.FeedbackSystemPrompt))
                .ForMember(d => d.LengthInMinutes, opt => opt.MapFrom(s => s.AssessmentTask.LengthInMinutes))
                .ForMember(d => d.LengthInInteractionCycles, opt => opt.MapFrom(s => s.AssessmentTask.LengthInInteractionCycles));
        }
    }
}
