namespace GFA.Medicals.Infrastructure.Models;

public class ServiceMedical : BaseEntity<int>
{
    public int MedicalOfficerId { get; set; }

    public string PeriodType { get; set; } = default!;

    public int PeriodYear { get; set; }

    public DateTime? DateStarted { get; set; }

    public DateTime? ExpectedRecoveryDate { get; set; }

    public DateTime? DateRecovered { get; set; }

    public int InjuryType { get; set; }

    public string Remarks { get; set; } = default!;

    public int PlayerClubId { get; set; }

    public float? Height { get; set; }

    public float? Weight { get; set; }

    public ServiceMemberClub? PlayerClub { get; set; }

    public ServiceMember? MedicalOfficer { get; set; }

    public IEnumerable<ServiceFileRecord>? Files { get; set; }
}
