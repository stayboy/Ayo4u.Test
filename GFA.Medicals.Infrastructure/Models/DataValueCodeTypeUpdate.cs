namespace GFA.Medicals.Infrastructure.Models;

public record DataValueCodeTypeUpdate : DataUpdate<int>
{
    public DataValueCodeTypeUpdate(int EntityId, DateTime CreatedAt) : base(EntityId, CreatedAt)
    {
    }

    public string CodeType { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}
