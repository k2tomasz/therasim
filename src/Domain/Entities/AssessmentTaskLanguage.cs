namespace Therasim.Domain.Entities;

public class AssessmentTaskLanguage : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Introductions { get; set; } = null!;
    public string Scenario { get; set; } = null!;
    public string Challenge { get; set; } = null!;
    public string Skills { get; set; } = null!;
    public string ClientPersona { get; set; } = null!;
    public string ClientSystemPrompt { get; set; } = null!;
    public string FeedbackSystemPrompt { get; set; } = null!;
    public Language Language { get; set; }
    public Guid AssessmentTaskId { get; set; }
    public AssessmentTask AssessmentTask { get; set; } = null!;
}