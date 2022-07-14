using Server.Shared.Models;

namespace GFA.Medicals.Data.Models
{
    internal class GFAUser : GFAIdModel<Guid, GFAUser>, IUserProfile<Guid>
    {
        public string Email { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;
    }
}
