namespace Ayo4u.Web.Shared.Models;

public record ApiAyoUserChange : ApiChange<Guid>
{
    public ApiAyoUserChange(Guid id, DateTime? created = null) : base(id, created)
    {
    }

    public string Email { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string FirstName { get; set; } = default!;
}
