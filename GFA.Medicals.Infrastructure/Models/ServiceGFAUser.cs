namespace GFA.Medicals.Infrastructure.Models
{
    public class ServiceGFAUser : BaseEntity<Guid>
    {
        public string EmailAddress { get; set; } = default!;

        public string FullName { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;
    }
}
