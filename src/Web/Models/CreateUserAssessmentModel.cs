
using System.ComponentModel.DataAnnotations;

namespace Therasim.Web.Models;

public class CreateUserAssessmentModel
{
    [Required(ErrorMessage = "Please select Assessment")]
    public string AssessmentId { get; set; } = null!;
    public string UserId { get; set; } = null!;
}
