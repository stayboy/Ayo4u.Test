namespace GFA.Medicals.Data.Models;

internal class Medical : GFAIdModel<int, Medical>
{
    public int ReportedById { get; set; }

    public string PeriodType { get; set; } = default!;

    public int PeriodYear { get; set; }

    public DateTime? DateStarted { get; set; }

    public DateTime? ExpectedRecoveryDate { get; set; }

    public DateTime? DateRecovered { get; set; }

    public int InjuryType { get; set; }

    public int? InjuryStatus { get; set; }

    public string Remarks { get; set; } = default!;

    public int PlayerClubId { get; set; }

    public float? Height { get; set; }

    public float? Weight { get; set; }

    public MemberClub PlayerClub { get; set; } = default!;

    public Member? ReportedBy { get; set; }

    public virtual ICollection<FileRecord> Files { get; set; } = new HashSet<FileRecord>();
}
