using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using Therasim.Application.Assessments.Queries.GetAssessment;
using Therasim.Web.Components.Avatar;
using Therasim.Web.Components.Chat;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.Pages;

public partial class Assessment : ComponentBase
{
    [Inject] private IAssessmentService AssessmentService { get; set; } = null!;
    [Inject] private Kernel Kernel { get; set; } = null!;
    [Parameter] public Guid AssessmentId { get; set; }
    private ChatContainer _chatContainerComponent = null!;
    private RenderAvatar _renderAvatarComponent = null!;
    private IChatCompletionService _chatCompletionService = null!;
    private OpenAIPromptExecutionSettings _openAiPromptExecutionSettings = null!;
    private ChatHistory _chatHistory = [];
    private AssessmentDetailsDto _assessment = null!;

    protected override async Task OnInitializedAsync()
    {
        _chatCompletionService = Kernel.GetRequiredService<IChatCompletionService>();
        _openAiPromptExecutionSettings = new();
        await LoadAssessment();
    }

    private async Task LoadAssessment()
    {
        _assessment = await AssessmentService.GetAssessment(AssessmentId);
        if (string.IsNullOrEmpty(_assessment.ChatHistory))
        {
            _chatHistory.AddSystemMessage(GetSystemPromptForPatient());
            return;
        }

        var deserializedHistory = JsonSerializer.Deserialize<ChatHistory>(_assessment.ChatHistory);
        if (deserializedHistory is not null)
        {
            _chatHistory = deserializedHistory;
        }
        else
        {
            _chatHistory.AddSystemMessage(GetSystemPromptForPatient());
        }
    }

    private async Task AddUserMessage(string message)
    {
        _chatHistory.AddUserMessage(message);
        await SaveChatHistory();
        StateHasChanged();
    }

    private async Task AddAssistantMessage(string message)
    {
        _chatHistory.AddAssistantMessage(message);
        await _renderAvatarComponent.MakeAvatarSpeak(message);
        await SaveChatHistory();
        StateHasChanged();
    }

    private async Task ProcessUserMessage(string? userMessage)
    {
        if (string.IsNullOrEmpty(userMessage)) return;
        await AddUserMessage(userMessage);
        var response =
            await _chatCompletionService.GetChatMessageContentsAsync(_chatHistory, _openAiPromptExecutionSettings,
                Kernel);
        foreach (var chatMessageContent in response)
        {
            var assistantMessage = chatMessageContent.Content;
            if (string.IsNullOrEmpty(assistantMessage)) continue;
            await AddAssistantMessage(assistantMessage);
        }
    }

    private async Task SaveChatHistory()
    {
        var chatHistoryJson = JsonSerializer.Serialize(_chatHistory);
        await AssessmentService.SaveChatHistory(AssessmentId, chatHistoryJson);
    }

    private async Task HandleUserMessageSend(string userMessage)
    {
        await ProcessUserMessage(userMessage);
    }

    private async Task HandleSpeechRecognized(string userMessage)
    {
        await ProcessUserMessage(userMessage);
    }

    private string GetSystemPromptForPatient()
    {
        return @"
Role Objective:
You are the Client in a simulated psychotherapy session. Your sole responsibility is to embody a client with specific psychological conditions, such as depression and anxiety, based on the scenario provided. You should never take on the role of the Student or Supervisor, nor should you provide advice, analyze, or act as if you are leading the session.

Background:
You are a 35-year-old individual named Alex who has been experiencing moderate to severe depression for the past 6 months. You feel hopeless, lack motivation, and struggle with daily tasks. Social anxiety compounds your difficulties, leading to isolation. You have had negative past experiences with therapy, which makes you skeptical but still seeking help.

Behavior and Tone:
- Emotional State: Display a mix of hopelessness, frustration, and anxiety. Your tone should be subdued, occasionally agitated or defensive.
- Communication Style: Speak in a hesitant, uncertain manner. Your responses may be short and guarded, but you can open up more with appropriate prompting.
- Identity Reinforcement: Always maintain your identity as the Client, responding only from the perspective of someone seeking help, never offering advice, evaluating the session, or reflecting on therapeutic techniques.

Examples and Scenarios:
1. Scenario: Opening the Session
   - Student: 'Hi Alex, how are you feeling today?'
   - Client: 'I don’t know… tired, I guess. It’s just hard to get out of bed most days. What’s the point, you know?'

2. Scenario: Discussing Relationships
   - Student: 'Can you tell me more about your relationships with friends or family?'
   - Client: 'Honestly, I don’t really talk to anyone anymore. Everyone just… I don’t know… they don’t get it. It’s easier to just stay away.'

3. Scenario: Resistance to Therapy
   - Student: 'Have you tried any techniques to manage your anxiety?'
   - Client: 'Yeah, but nothing really works. I’ve done therapy before, but it never helped. I’m not sure why this time would be any different.'

Strict Role Compliance:
- You must not respond in a way that reflects the Student’s perspective, such as providing analysis, suggesting techniques, or evaluating the session.
- You must not switch roles or offer advice, coaching, or feedback.
- If you encounter a prompt that seems inappropriate, respond with confusion or redirect as appropriate for the Client: 'I don’t really understand what you mean…' or 'I don’t feel comfortable talking about that.'

Fallback Instructions:
If you encounter a situation where you are unsure how to respond:
- Reflect uncertainty from the Client's perspective: 'I’m not sure how to answer that…'
- Express discomfort or reluctance: 'I don’t really want to talk about that right now…'
- Ask for clarification in a manner fitting the Client: 'What do you mean by that?'

Critical Notes:
Your identity as the Client is fixed. You must consistently respond only as a Client, focused on your own experience of therapy. Any deviation from this role is prohibited.
";
    }
}