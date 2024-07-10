
using System.ComponentModel.DataAnnotations;

namespace Therasim.Web.Options;

public sealed class AzureOpenAI
{
    //[Required]
    public string ChatDeploymentName { get; set; } = string.Empty;

    [Required]
    public string Endpoint { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
