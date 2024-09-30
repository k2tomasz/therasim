using Microsoft.AspNetCore.Components;

namespace Therasim.Web.Pages.UserAssessments;

public partial class Details : ComponentBase
{
    [Parameter] public Guid UserAssessmentId { get; set; }
}