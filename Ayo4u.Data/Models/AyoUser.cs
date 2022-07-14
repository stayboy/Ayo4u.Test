namespace Ayo4u.Data.Models;

internal class AyoUser : AyoIdModel<Guid, AyoUser>, IUserProfile<Guid>
{
    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
}
