namespace Therasim.Domain.Entities;

public class Session : BaseAuditableEntity
{
    public bool IsActive { get; set; }
    public Guid SimulationId { get; set; }
    public Simulation Simulation { get; set; } = null!;
    public string? FeedbackHistory { get; set; }
    public string? ChatHistory { get; set; }
}