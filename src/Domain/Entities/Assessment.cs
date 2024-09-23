namespace Therasim.Domain.Entities;

public class Assessment : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Language Language { get; set; }
    public string FeedbackSystemPrompt { get; set; } = null!;
    public IList<AssessmentTask> AssessmentTasks { get; private set; } = new List<AssessmentTask>();
    public IList<UserAssessment> UserAssessments { get; private set; } = new List<UserAssessment>();
}