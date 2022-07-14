namespace GFA.Medicals.Infrastructure.Queries;

public class ClubSearchParameters : SearchParameters<int>
{
    public int? CountryId { get; set; }

    public string? Division { get; set; }
}
