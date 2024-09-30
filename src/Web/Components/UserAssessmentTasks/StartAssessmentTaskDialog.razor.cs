using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Therasim.Application.UserAssessmentTasks.Queries.GetUserAssessmentTask;

namespace Therasim.Web.Components.UserAssessmentTasks
{
    public partial class StartAssessmentTaskDialog : IDialogContentComponent<UserAssessmentTaskDetailsDto>
    {
        [Parameter] public UserAssessmentTaskDetailsDto Content { get; set; } = default!;
        [CascadingParameter] public FluentDialog? Dialog { get; set; }
    }
}
