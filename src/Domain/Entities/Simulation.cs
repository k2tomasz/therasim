namespace Therasim.Domain.Entities;

public class Simulation : BaseAuditableEntity
{
    public string UserId { get; set; } = null!;
    public Guid PersonaId { get; set; }
    public Persona Persona { get; set; } = null!;
    public FeedbackType FeedbackType { get; set; }
    public Language Language { get; set; }
    public IList<Session> Sessions { get; private set; } = new List<Session>();
}
