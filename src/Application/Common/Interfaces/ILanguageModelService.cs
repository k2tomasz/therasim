using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Domain.Enums;

namespace Therasim.Application.Common.Interfaces;

public interface ILanguageModelService
{
    Task<string> GenerateTranscript(string scenario, string challenge, Language language);
    Task<string> GetChatMessageContentsAsync(ChatHistory chatHistory);
    Task<string> GenerateAssessmentTaskFeedback(string transcript, string scenario, string challenge, string skills, string assessmentCriteria, string feedbackSystemPrompt, Language language = Language.English);
    string GetSystemPromptForPatient();
    string GetSystemPromptForSessionFeedback();
    string GetSystemPromptForAssessmentFeedback();
    string GetSystemPromptForStudent();
    string GetPolishSystemPromptForAssessmentFeedback();
    string GetEnglishSystemPromptForAssessmentFeedback();
}