using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Sessions.Queries.GetSession;

public class SessionDto
{
    public Guid Id { get; set; }
    public Guid PersonaId { get; set; }
    public FeedbackType FeedbackType { get; set; }
    public Language Language { get; set; }
    public string? FeedbackHistory { get; set; }
    public string? ChatHistory { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Session, SessionDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.FeedbackHistory, opt => opt.MapFrom(s => s.FeedbackHistory))
                .ForMember(d => d.ChatHistory, opt => opt.MapFrom(s => s.ChatHistory))
                .ForMember(d => d.PersonaId, opt => opt.MapFrom(s => s.Simulation.PersonaId))
                .ForMember(d => d.FeedbackType, opt => opt.MapFrom(s => s.Simulation.FeedbackType))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Simulation.Language));
        }
    }
}