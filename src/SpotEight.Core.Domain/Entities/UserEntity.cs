using SpotEight.Core.Domain.Common;

namespace SpotEight.Core.Domain.Entities;

public class UserEntity : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
