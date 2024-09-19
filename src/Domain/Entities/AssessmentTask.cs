namespace Therasim.Domain.Entities;

public class AssessmentTask : BaseAuditableEntity
{
    public string Objective { get; set; } = null!;
    public string PersonaSystemPrompt { get; set; } = null!;
    public string FeedbackSystemPrompt { get; set; } = null!;
    public Language Language { get; set; }
    public int? LengthInMinutes { get; set; }
    public int? LengthInInteractionCycles { get; set; }
    public Guid AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;
    public IList<UserAssessmentTask> UserAssessmentTasks { get; private set; } = new List<UserAssessmentTask>();

}