using Microsoft.AspNetCore.Components;
using Therasim.Application.AssessmentTasks.Queries.GetAssessmentTasks;
using Therasim.Domain.Enums;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.AssessmentTasks;

public partial class ListAssessmentTasks : ComponentBase
{
    [Inject] private IAssessmentTaskService AssessmentTaskService { get; set; } = null!;
    [Parameter] public Guid AssessmentId { get; set; }
    [Parameter] public Language Language { get; set; }
    private IList<AssessmentTaskDto> _assessmentTasks = new List<AssessmentTaskDto>();

    protected override async Task OnInitializedAsync()
    {
        await GetAssessmentTasks();
    }

    public async Task GetAssessmentTasks()
    {
        _assessmentTasks = await AssessmentTaskService.GetAssessmentTasks(AssessmentId, Language);
        StateHasChanged();
    }
}
