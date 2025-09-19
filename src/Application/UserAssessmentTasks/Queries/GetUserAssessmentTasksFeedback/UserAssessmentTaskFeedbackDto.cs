using Microsoft.SemanticKernel.ChatCompletion;
using System.Text.Json;
using Therasim.Domain.Entities;

namespace Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTasksFeedback;

public class UserAssessmentTaskFeedbackDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Feedback { get; set; }
    public string? ChatHistory { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserAssessmentTask, UserAssessmentTaskFeedbackDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.AssessmentTask.AssessmentTaskLanguages.Where(x => x.Language == s.Language).First().Name))
                .ForMember(d => d.Feedback, opt => opt.MapFrom(s => s.Feedback))
                .ForMember(d => d.ChatHistory, opt => opt.MapFrom(s => s.ChatHistory));
        }
    }

    public ChatHistory GetChatHistory()
    {
        if (ChatHistory is null)
        {
            return new ChatHistory();
        }
        var deserializedHistory = JsonSerializer.Deserialize<ChatHistory>(ChatHistory);
        if (deserializedHistory is not null)
        {
            return deserializedHistory;
        }
        return new ChatHistory();
    }
}
