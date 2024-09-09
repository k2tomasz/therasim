namespace Therasim.Domain.Entities;
public class Assessment : BaseAuditableEntity
{
    public string UserId { get; set; } = null!;
    public Guid PersonaId { get; set; }
    public Persona Persona { get; set; } = null!;
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    public Language Language { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Feedback { get; set; }
    public string? ChatHistory { get; set; }
}