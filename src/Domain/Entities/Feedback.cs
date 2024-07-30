namespace Therasim.Domain.Entities;

public class Feedback : BaseAuditableEntity
{
    public string Content { get; set; } = null!;
    public string Message { get; set; } = null!;
    public bool? IsUseful { get; set; }
    public Guid SessionId { get; set; }
    public Session Session { get; set; } = null!;
}