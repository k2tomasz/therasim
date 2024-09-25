using Microsoft.AspNetCore.Components;
using Therasim.Application.AssessmentTasks.Queries.GetAssessmentTasks;

namespace Therasim.Web.Components.AssessmentTasks;

public partial class AssessmentTaskDetails : ComponentBase
{
    [Parameter] public AssessmentTaskDto AssessmentTask { get; set; } = null!;
}
