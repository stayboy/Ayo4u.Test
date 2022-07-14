namespace GFA.Medicals.Infrastructure.Queries;

public class MemberSearchParameters : SearchParameters<int>
{
    public string? MemberType { get; set; }

    public string? Gender { get; set; }

    public int? ClubId { get; set; }

    public int? ClubCountryId { get; set; }

    public bool? IsLatestClub { get; set; }

    public int? InjuryType { get; set; }

    public bool? IsFit { get; set; }

    public int? MinAgeYears { get; set; }

    public int? MaxAgeYears { get; set; }

    public string? InjuryPeriod { get; set; }

    public int? InjuryYear { get; set; }

    public bool? LoadMedicals { get; set; }

    public bool? LoadClubs { get; set; }
}
