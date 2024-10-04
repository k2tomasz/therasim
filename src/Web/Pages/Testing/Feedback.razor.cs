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
    private string _assessmentFocus = "The therapist’s paraphrase will be evaluated based on how well it captures the essence of the client’s concerns, organizes the scattered elements of the dialogue, and conveys empathy without distorting or minimizing the client’s feelings. The paraphrasing should help the client feel validated and understood, and ideally, create space for the client to expand on or clarify their thoughts.";
    private string _taskScenario = "The client, a middle-aged individual, expresses feeling overwhelmed by several recent life events, including the death of a close family member, job insecurity, and health issues. The client begins to speak rapidly, sharing how everything is \"piling up\" and that they feel like they are \"drowning\" under the weight of it all. They mention feeling guilty for not \"holding it together\" and not being able to manage their emotions better, and they jump from one topic to another, describing their stress in a disorganized way.";
    private string _taskChallenge = "The therapist must use paraphrasing to restate the client’s thoughts and feelings in a clear, concise way, demonstrating understanding and providing the client with a sense of being heard. The paraphrase should help the client feel more organized in their expression and open up space for deeper reflection or clarification.";
    private string _taskSkills = "Active listening: Accurately picking up on the key points and emotional undertones of the client’s communication.\nClarification: Providing a restatement that helps organize the client’s scattered thoughts and emotions.\nEmpathy: Conveying understanding and nonjudgmental support through the paraphrase.";
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
        _feedback = await LanguageModelService.GenerateAssessmentTaskFeedback(_transcript, _taskScenario,
            _taskChallenge, _taskSkills, _assessmentFocus, _feedbackPrompt, _language);
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