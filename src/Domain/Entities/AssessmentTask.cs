namespace Therasim.Domain.Entities;

public class AssessmentTask : BaseAuditableEntity
{
    public int Order { get; set; }
    public int? LengthInMinutes { get; set; }
    public int? LengthInInteractionCycles { get; set; }
    public Guid AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;
    public IList<AssessmentTaskLanguage> AssessmentTaskLanguages { get; private set; } = new List<AssessmentTaskLanguage>();
    public IList<UserAssessmentTask> UserAssessmentTasks { get; private set; } = new List<UserAssessmentTask>();

}