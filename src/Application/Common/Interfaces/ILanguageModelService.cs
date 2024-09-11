using Microsoft.SemanticKernel.ChatCompletion;

namespace Therasim.Application.Common.Interfaces;

public interface ILanguageModelService
{
    Task<string> GetChatMessageContentsAsync(ChatHistory chatHistory);
    Task<string> GenerateAssessmentFeedback(string? transcript);
    string GetSystemPromptForPatient();
    string GetSystemPromptForSessionFeedback();
    string GetSystemPromptForAssessmentFeedback();
    string GetSystemPromptForStudent();
}