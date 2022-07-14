namespace GFA.Medicals.Infrastructure.Models;

public abstract record DataUpdate<T>(T EntityId, DateTime CreatedAt) where T : struct
{
    public ServiceGFAUser GFAUser { get; set; }

    public DateTime UpdatedAt { get; set; }
}
