using Microsoft.AspNetCore.Components;

namespace Therasim.Web.Pages.Assessments;

public partial class Details : ComponentBase
{
    [Parameter] public Guid AssessmentId { get; set; }
}