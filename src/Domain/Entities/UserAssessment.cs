namespace Therasim.Domain.Entities;
public class UserAssessment : BaseAuditableEntity
{
    public string UserId { get; set; } = null!;
    public Guid AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;
    public string? Feedback { get; set; }
}