using System.ComponentModel.DataAnnotations;

namespace Therasim.Web.Models;

public class UserMessageModel
{
    [Required]
    [StringLength(1000)]
    public string? Message { get; set; } = null!;
}