namespace Therasim.Domain.Entities;

public class PsychProblem : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IList<Simulation> Simulations { get; private set; } = new List<Simulation>();
}
