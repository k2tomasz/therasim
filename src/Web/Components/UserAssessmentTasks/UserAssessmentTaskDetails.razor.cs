using Microsoft.AspNetCore.Components;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTasks;

namespace Therasim.Web.Components.UserAssessmentTasks;

public partial class UserAssessmentTaskDetails : ComponentBase
{
    [Parameter] public UserAssessmentTaskDto UserAssessmentTask { get; set; } = null!;
}
