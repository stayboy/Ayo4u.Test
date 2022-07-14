namespace GFA.Medicals.Infrastructure.Models;

public record DataClubUpdate : DataUpdate<int>
{
    public DataClubUpdate(int EntityId, DateTime CreatedAt) : base(EntityId, CreatedAt)
    {
    }

    public int CountryId { get; set; }

    public string Name { get; set; } = default!;

    public string? Initials { get; set; }

    public string? Division { get; set; }

    public DateTime? Effective { get; set; }
}
