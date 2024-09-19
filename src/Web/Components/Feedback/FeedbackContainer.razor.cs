using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Application.Common.Interfaces;
using Therasim.Web.Services.Interfaces;

namespace Therasim.Web.Components.Feedback
{
    public partial class FeedbackContainer : ComponentBase
    {
        [Inject] private ISessionService SessionService { get; set; } = null!;
        [Inject] private ILanguageModelService LanguageModelService { get; set; } = null!;
        [Parameter] public Guid SessionId { get; set; }
        private ChatHistory _feedbackHistory = [];

        protected override async Task OnInitializedAsync()
        {
            await LoadSession();
        }

        private async Task LoadSession()
        {
            var _session = await SessionService.GetSession(SessionId);
            if (string.IsNullOrEmpty(_session.FeedbackHistory))
            {
                _feedbackHistory.AddSystemMessage(LanguageModelService.GetSystemPromptForSessionFeedback());
                return;
            }

            var deserializedHistory = JsonSerializer.Deserialize<ChatHistory>(_session.FeedbackHistory);
            if (deserializedHistory is not null)
            {
                _feedbackHistory = deserializedHistory;
            }
            else
            {
                _feedbackHistory.AddSystemMessage(LanguageModelService.GetSystemPromptForSessionFeedback());
            }
        }

        private async Task AddUserMessage(string message)
        {
            _feedbackHistory.AddUserMessage(message);
            await SaveFeedbackHistory();
        }

        private async Task AddAssistantMessage(string message)
        {
            _feedbackHistory.AddAssistantMessage(message);
            await SaveFeedbackHistory();
            StateHasChanged();
        }

        //public async Task GetFeedbackForAssistantMessage(string message)
        //{
        //    var sessionMessage = $"Client: {message}";
        //    await GetFeedback(sessionMessage);
        //}

        //public async Task GetFeedbackForUserMessage(string message)
        //{
        //    var sessionMessage = $"Student: {message}";
        //    await GetFeedback(sessionMessage);
        //}

        public async Task GetFeedback(string studentMessage, string clientMessage)
        {
            var sessionMessage = $"Student: {studentMessage}; Client: {clientMessage}";
            await AddUserMessage(sessionMessage);
            var response = await LanguageModelService.GetChatMessageContentsAsync(_feedbackHistory);
            if (string.IsNullOrEmpty(response)) return;
            await AddAssistantMessage(response);
        }

        private async Task SaveFeedbackHistory()
        {
            var feedbackHistoryJson = JsonSerializer.Serialize(_feedbackHistory);
            await SessionService.SaveFeedbackHistory(SessionId, feedbackHistoryJson);
        }
    }
}
