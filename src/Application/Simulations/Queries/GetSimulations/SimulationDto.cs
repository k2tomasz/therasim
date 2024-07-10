using Therasim.Domain.Entities;

namespace Therasim.Application.Simulations.Queries.GetSimulations;

public class SimulationDto
{
    public Guid Id { get; set; }
    public string Skill { get; set; } = "Communication";
    public string Persona { get; set; } = "Alex, 25, recent college graduate";
    public string Problem { get; set; } = "Depression";
    public string FeedbackType { get; set; } = "Extensive";

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Simulation, SimulationDto>();
        }
    }
}
