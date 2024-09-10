using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Simulations.Queries.GetSimulations;

public class SimulationDto
{
    public Guid Id { get; set; }
    public string Persona { get; set; } = string.Empty;
    public FeedbackType FeedbackType { get; set; }
    public Language Language { get; set; }
    public Guid? ActiveSessionId { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Simulation, SimulationDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Persona, opt => opt.MapFrom(s => s.Persona.Name))
                .ForMember(d => d.FeedbackType, opt => opt.MapFrom(s => s.FeedbackType))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Language))
                .ForMember(d => d.ActiveSessionId, opt => opt.MapFrom(s => s.Sessions.Count == 0 ? null : (Guid?)s.Sessions.Select(x=>x.Id).First()));
        }
    }
}
