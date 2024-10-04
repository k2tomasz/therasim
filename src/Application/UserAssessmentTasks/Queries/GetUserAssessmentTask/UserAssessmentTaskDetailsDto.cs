using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;

public class UserAssessmentTaskDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Scenario { get; set; } = null!;
    public string Challenge { get; set; } = null!;
    public string ClientInitialDialogue { get; set; } = string.Empty;
    public string ClientSystemPrompt { get; set; } = null!;
    public Language Language { get; set; }
    public int? LengthInMinutes { get; set; }
    public int? LengthInInteractionCycles { get; set; }
    public string? Feedback { get; set; }
    public string? ChatHistory { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserAssessmentTask, UserAssessmentTaskDetailsDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.AssessmentTask.AssessmentTaskLanguages.First().Name))
                .ForMember(d => d.Scenario, opt => opt.MapFrom(s => s.AssessmentTask.AssessmentTaskLanguages.First().Scenario))
                .ForMember(d => d.Challenge, opt => opt.MapFrom(s => s.AssessmentTask.AssessmentTaskLanguages.First().Challenge))
                .ForMember(d => d.ClientSystemPrompt, opt => opt.MapFrom(s => s.AssessmentTask.AssessmentTaskLanguages.First().ClientSystemPrompt))
                .ForMember(d => d.LengthInMinutes, opt => opt.MapFrom(s => s.AssessmentTask.LengthInMinutes))
                .ForMember(d => d.LengthInInteractionCycles, opt => opt.MapFrom(s => s.AssessmentTask.LengthInInteractionCycles))
                .ForMember(d => d.Feedback, opt => opt.MapFrom(s => s.Feedback))
                .ForMember(d => d.ChatHistory, opt => opt.MapFrom(s => s.ChatHistory));
        }
    }
}
