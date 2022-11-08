namespace GFA.Medicals.Data.Models;

internal class GFAUser : GFAIdModel<Guid, GFAUser>, IUserProfile<Guid>, IEquatable<GFAUser>
{
    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public bool Equals(GFAUser? other)
    {
        if (other != null)
        {
            if (ReferenceEquals(this, other)) return true;

            return (Id == other?.Id) || string.Equals(Email, other!.Email);
        }

        return false;
    }
}
