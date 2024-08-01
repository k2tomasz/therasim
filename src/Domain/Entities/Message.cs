namespace Therasim.Domain.Entities;

public class Message : BaseAuditableEntity
{
    public string Content { get; set; } = null!;
    public MessageAuthorRole Role { get; set; }
    public Guid SessionId { get; set; }
    public Session Session { get; set; } = null!;
}
