using Microsoft.AspNetCore.Components;
using Microsoft.SemanticKernel.ChatCompletion;
using Therasim.Web.Models;

namespace Therasim.Web.Components.Chat
{
    public partial class ChatContainer : ComponentBase
    {
        [Parameter] public EventCallback<string> OnUserMessageSend { get; set; }
        [Parameter] public ChatHistory ChatHistory { get; set; } = [];
        [SupplyParameterFromForm] private UserMessageModel UserMessageModel { get; set; } = new();

        private async Task HandleValidSubmitAsync()
        {
            var userMessage = UserMessageModel.Message;
            if (string.IsNullOrWhiteSpace(userMessage)) return;
            UserMessageModel = new UserMessageModel();
            await OnUserMessageSend.InvokeAsync(userMessage);
        }
    }
}
