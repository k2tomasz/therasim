namespace Therasim.Domain.Entities;

public class Simulation : BaseEntity
{
    public string UserId { get; set; } = null!;
    public Guid PersonaId { get; set; }
    public Persona Persona { get; set; } = null!;
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    public Guid ProblemId { get; set; }
    public Problem Problem { get; set; } = null!;
    public FeedbackType FeedbackType { get; set; }
    public Language Language { get; set; }
    public IList<Conversation> Conversations { get; private set; } = new List<Conversation>();
}
