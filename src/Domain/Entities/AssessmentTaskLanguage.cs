namespace Therasim.Domain.Entities;

public class AssessmentTaskLanguage : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Instructions { get; set; } = string.Empty;
    public string Scenario { get; set; } = string.Empty;
    public string Challenge { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string ClientPersona { get; set; } = string.Empty;
    public string ClientSystemPrompt { get; set; } = string.Empty;
    public string FeedbackSystemPrompt { get; set; } = string.Empty;
    public Language Language { get; set; }
    public Guid AssessmentTaskId { get; set; }
    public AssessmentTask AssessmentTask { get; set; } = null!;
}