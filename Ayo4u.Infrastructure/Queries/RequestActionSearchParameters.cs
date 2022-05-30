
namespace Ayo4u.Infrastructure.Queries;

public class RequestActionSearchParameters
{
    public int[]? Ids { get; set; }

    public Guid? RequestAyoUserId { get; set; }

    public string? RequesterName { get; set; }

    public string? InUnitType { get; set; }

    public string? OutUnitType { get; set; }

    public DateTime? StartCreatedAt { get; set; }

    public DateTime? EndCreatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
}
