using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Therasim.Application.Common.Interfaces;
using Therasim.Domain.Enums;

namespace Therasim.Web.Pages.Testing;

public partial class Feedback : ComponentBase
{
    [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;

    private string _feedback = string.Empty;
    private string _feedbackPrompt = string.Empty;
    private string _taskName = "Alliance Rupture: Client Feels Unheard";
    private string _taskScenario = "The client feels frustrated, believing that the therapist is not listening or grasping their main concerns.";
    private string _taskChallenge = "The therapist must validate the client’s experience, acknowledge the rupture, and demonstrate effective repair to restore trust.";
    private string _taskSkills = "Empathy, alliance rupture-repair, verbal fluency.";
    private string _transcript = string.Empty;
    private Language _language = Language.English;
    private bool _disableButtons;
    private void LoadFeedbackPrompt()
    {
        if (_language == Language.English)
        {
            _feedbackPrompt = LanguageModelService.GetEnglishSystemPromptForAssessmentFeedback();
        }
        else
        {
            _feedbackPrompt = LanguageModelService.GetPolishSystemPromptForAssessmentFeedback();
        }
    }

    private async Task GenerateTranscript(MouseEventArgs obj)
    {
        _disableButtons = true;
        _transcript = await LanguageModelService.GenerateTranscript(_taskScenario, _taskChallenge, _language);
        _disableButtons = false;
        StateHasChanged();
    }

    private async Task GenerateFeedback(MouseEventArgs obj)
    {
        _disableButtons = true;
        _feedback = await LanguageModelService.GenerateTaskFeedback(_transcript, _taskName, _taskScenario,
            _taskChallenge, _taskSkills, _feedbackPrompt, _language);
        _disableButtons = false;
        StateHasChanged();
    }

    private void LanguageChanged(Language language)
    {
        _language = language;
        StateHasChanged();
    }

    private void LoadDefaultFeedbackPrompt(MouseEventArgs obj)
    {
        LoadFeedbackPrompt();
        StateHasChanged();
    }
}