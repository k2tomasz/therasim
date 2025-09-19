namespace Therasim.Domain.Entities;

public class Skill : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
