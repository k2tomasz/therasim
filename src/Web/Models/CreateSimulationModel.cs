
using System.ComponentModel.DataAnnotations;

namespace Therasim.Web.Models;

public class CreateSimulationModel
{
    public string UserId { get; set; } = null!;
    [Required(ErrorMessage = "Persona is Required")]
    public string PersonaId { get; set; } = null!;

    [Required(ErrorMessage = "Problem is Required")]
    public string ProblemId { get; set; } = null!;

    [Required(ErrorMessage = "Skill is Required")]
    public string SkillId { get; set; } = null!;

    [Required(ErrorMessage = "Feedback is Required")]
    public string FeedbackType { get; set; } = null!;

    [Required(ErrorMessage = "Language is Required")]
    public string Language { get; set; } = null!;
}
