namespace Therasim.Web.Models;

public class CreateSimulationModel
{
    public string PersonaId { get; set; } = null!;
    public string PsychProblemId { get; set; } = null!;
    public string SkillId { get; set; } = null!;
    public int FeedbackType { get; set; }
}
