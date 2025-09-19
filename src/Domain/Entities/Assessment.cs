namespace Therasim.Domain.Entities;

public class Assessment : BaseAuditableEntity
{
    public IList<AssessmentLanguage> AssessmentLanguages { get; private set; } = new List<AssessmentLanguage>();
    public IList<AssessmentTask> AssessmentTasks { get; private set; } = new List<AssessmentTask>();
    public IList<UserAssessment> UserAssessments { get; private set; } = new List<UserAssessment>();
}