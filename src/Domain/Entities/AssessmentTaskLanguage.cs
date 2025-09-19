namespace Therasim.Domain.Entities;

public class AssessmentTaskLanguage : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Scenario { get; set; } = string.Empty;
    public string Challenge { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string ClientInitialDialogue { get; set; } = string.Empty;
    public string EffectiveResponse { get; set; } = string.Empty;
    public string AssessmentCriteria { get; set; } = string.Empty;
    public string ClientSystemPrompt { get; set; } = string.Empty;
    public string FeedbackSystemPrompt { get; set; } = string.Empty;
    public Language Language { get; set; }
    public Guid AssessmentTaskId { get; set; }
    public AssessmentTask AssessmentTask { get; set; } = null!;
}