namespace Therasim.Web.Models
{
    public class ChatMessage
    {
        public string ChatId { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string Text { get; set; } = null!;
        public bool IsUser { get; set; }
        public string Id { get; set; } = null!;
        public string Prompt { get; set; } = null!;
    }
}
