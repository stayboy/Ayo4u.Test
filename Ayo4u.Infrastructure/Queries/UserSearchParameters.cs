namespace Ayo4u.Infrastructure.Queries;

public class UserSearchParameters
{
    public Guid[]? UserIds { get; set; }

    public string? Email { get; set; }

    public string? UserName { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? StartCreatedAt { get; set; }

    public DateTime? EndCreatedAt { get; set; }
}
