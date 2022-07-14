namespace GFA.Medicals.Infrastructure.Models
{
    public class ServiceClub : BaseEntity<int>
    {
        public int CountryId { get; set; }

        public string ClubName { get; set; } = default!;

        public string? Initials { get; set; }

        public string? ClubDivision { get; set; }

        public DateTime? Effective { get; set; }

        public ServiceValueCodeType? ClubCountry { get; set; }

        public IEnumerable<ServiceMember>? Players { get; set; }
    }
}
