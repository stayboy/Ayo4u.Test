namespace GFA.Medicals.Infrastructure.Queries;

public class MedicalSearchParameters : SearchParameters<int>
{
    public int? InjuryType { get; set; }

    public int? MedicalOfficerId { get; set; }

    public int? InjuryYear { get; set; }

    public string? InjuryPeriod { get; set; }

    public int? ClubId { get; set; }

    public int? PlayerId { get; set; }

    public bool? IsRecovered { get; set; }

    public int? InjuryStatus { get; set; }

    public bool? ShowPlayer { get; set; }
}
