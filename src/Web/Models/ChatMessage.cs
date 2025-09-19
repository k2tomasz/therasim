namespace Therasim.Web.Models
{
    public class ChatMessageModel
    {
        public string Text { get; set; } = null!;
        public bool IsUser { get; set; }
    }
}
