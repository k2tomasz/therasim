using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Domain.Enums;

namespace Therasim.Application.Common.Interfaces;

public interface ILanguageModelService
{
    Task<string> GenerateTranscript(string scenario, string challenge, Language language);
    Task<string> GenerateTaskFeedback(string transcript, string taskName, string scenario, string challenge, string skills, string feedbackSystemPrompt, Language language);
    Task<string> GetChatMessageContentsAsync(ChatHistory chatHistory);
    Task<string> GenerateAssessmentFeedback(string? transcript, string feedbackSystemPrompt);
    string GetSystemPromptForPatient();
    string GetSystemPromptForSessionFeedback();
    string GetSystemPromptForAssessmentFeedback();
    string GetSystemPromptForStudent();
    string GetPolishSystemPromptForAssessmentFeedback();
    string GetEnglishSystemPromptForAssessmentFeedback();
}