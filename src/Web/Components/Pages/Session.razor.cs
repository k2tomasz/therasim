using Microsoft.AspNetCore.Components;
using Therasim.Web.Models;


namespace Therasim.Web.Components.Pages
{
    public partial class Session : ComponentBase
    {
        [Parameter] public Guid SessionId { get; set; }
        private Feedback.Feedback FeedbackComponent { get; set; } = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            // Initialization logic if needed
        }

        private async Task HandleChatUpdated(ChatMessage message)
        {
            await FeedbackComponent.GetFeedback(message);
        }
    }

}
