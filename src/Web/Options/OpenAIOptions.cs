using System.ComponentModel.DataAnnotations;

namespace Therasim.Web.Options;

public sealed class OpenAIOptions
{
    [Required]
    public string ChatModelId { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
