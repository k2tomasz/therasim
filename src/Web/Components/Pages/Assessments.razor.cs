using Microsoft.AspNetCore.Components;
using Therasim.Web.Components.Assessments;

namespace Therasim.Web.Components.Pages;

public partial class Assessments : ComponentBase
{
    private ListAssessments ListAssessmentsComponent { get; set; } = null!;

    private async Task HandleAssessmentCreated()
    {
        await ListAssessmentsComponent.GetAssessments();
    }
}