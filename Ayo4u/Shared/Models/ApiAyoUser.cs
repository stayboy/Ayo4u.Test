namespace Ayo4u.Web.Shared.Models
{
    public class ApiAyoUser : ApiBaseEntity<Guid>
    {
        public string EmailAddress { get; set; } = default!;

        public string UserFullName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string FirstName { get; set; } = default!;
    }
}
