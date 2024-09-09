namespace Therasim.Domain.Entities;

public class Skill : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IList<Simulation> Simulations { get; private set; } = new List<Simulation>();
    public IList<Assessment> Assessments { get; private set; } = new List<Assessment>();
}
