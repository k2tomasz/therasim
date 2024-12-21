using System.ComponentModel.DataAnnotations;

namespace Therasim.Web.Models
{
    public class CreateUserProfile
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; } = null!;
    }
}
