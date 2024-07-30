namespace Therasim.Domain.Entities;

public class Session : BaseAuditableEntity
{
    public bool IsActive { get; set; }
    public Guid SimulationId { get; set; }
    public Simulation Simulation { get; set; } = null!;
    public IList<Message> Messages { get; private set; } = new List<Message>();
    public IList<Feedback> Feedbacks { get; private set; } = new List<Feedback>();
}