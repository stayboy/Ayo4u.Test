namespace GFA.Medicals.Infrastructure.Models;

public record DataMedicalUpdate : DataUpdate<int>
{
    public DataMedicalUpdate(int entityId, DateTime createdAt) : base(entityId, createdAt)
    {
    }

    public MedicalInfo? History { get; set; }

    public IEnumerable<DataFileRecordUpdate>? Uploads { get; set; }
}

public record MedicalInfo(int InjuryType, int PlayerClubId, string PeriodType, int PeriodYear)
{
    public int ReportedById { get; set; }

    public DateTime? DateStarted { get; set; }

    public DateTime? ExpectedRecoveryDate { get; set; }

    public DateTime? DateRecovered { get; set; }

    public string Remarks { get; set; } = default!;

    public float? Height { get; set; }

    public float? Weight { get; set; }
}