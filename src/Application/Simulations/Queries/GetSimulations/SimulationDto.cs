using Therasim.Domain.Entities;
using Therasim.Domain.Enums;

namespace Therasim.Application.Simulations.Queries.GetSimulations;

public class SimulationDto
{
    public Guid Id { get; set; }
    public string Skill { get; set; } = string.Empty;
    public string Persona { get; set; } = string.Empty;
    public string Problem { get; set; } = string.Empty;
    public FeedbackType FeedbackType { get; set; }
    public Language Language { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Simulation, SimulationDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Skill, opt => opt.MapFrom(s => s.Skill.Name))
                .ForMember(d => d.Persona, opt => opt.MapFrom(s => s.Persona.Name))
                .ForMember(d => d.Problem, opt => opt.MapFrom(s => s.PsychProblem.Name))
                .ForMember(d => d.FeedbackType, opt => opt.MapFrom(s => s.FeedbackType))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Language));
        }
    }
}
