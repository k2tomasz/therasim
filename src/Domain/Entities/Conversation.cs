namespace Therasim.Domain.Entities;

public class Conversation : BaseEntity
{
    public string UserId { get; set; } = null!;
    public Guid SimulationId { get; set; }
    public Simulation Simulation { get; set; } = null!;
    public string ChatThreadId { get; set; } = null!;
    public string FeedbackThreadId { get; set; } = null!;
}
