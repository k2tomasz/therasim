namespace Therasim.Domain.Entities;

public class UserAssessmentTask : BaseAuditableEntity
{
    public string UserId { get; set; } = null!;
    public Guid UserAssessmentId { get; set; }
    public UserAssessment UserAssessment { get; set; } = null!;
    public Guid AssessmentTaskId { get; set; }
    public AssessmentTask AssessmentTask { get; set; } = null!;
    public Language Language { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Order { get; set; }
    public string? Feedback { get; set; }
    public string? ChatHistory { get; set; }
}