using System.ComponentModel.DataAnnotations;

namespace Therasim.Web.Models;

public class UserMessageModel
{
    [Required]
    [StringLength(500)]
    public string? Message { get; set; } = null!;
}