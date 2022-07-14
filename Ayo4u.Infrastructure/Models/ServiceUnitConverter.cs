namespace Ayo4u.Infrastructure.Models
{
    public class ServiceUnitConverter : BaseEntity<int>
    {
        public string InUnitType { get; set; } = default!;

        public string OutUnitType { get; set; } = default!;

        public float Multiplier { get; set; }

        public Formulae? Formula { get; set; }

        public IEnumerable<ServiceRequestAction>? RequestLogs { get; set; }
    }
}
