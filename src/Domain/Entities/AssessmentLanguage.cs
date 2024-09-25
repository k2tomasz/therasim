namespace Therasim.Domain.Entities;
public class AssessmentLanguage : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Instructions { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string FeedbackSystemPrompt { get; set; } = null!;
    public Language Language { get; set; }
    public Guid AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;
}