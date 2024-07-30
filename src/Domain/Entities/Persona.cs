namespace Therasim.Domain.Entities;

public class Persona : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Background { get; set; } = null!;
    public IList<Simulation> Simulations { get; private set; } = new List<Simulation>();
}
