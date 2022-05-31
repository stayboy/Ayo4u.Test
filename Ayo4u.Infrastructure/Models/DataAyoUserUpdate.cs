namespace Ayo4u.Infrastructure.Models;

public record DataAyoUserUpdate (Guid Id, DateTime CreatedAt)
{
    public ServiceAyoUser? AyoUser { get; set; }

    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
}
