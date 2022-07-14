
namespace Ayo4u.Infrastructure.Models
{
    public class ServiceAyoUser : BaseEntity<Guid>
    {
        public string EmailAddress { get; set; } = default!;

        public string FullName { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;
    }
}
