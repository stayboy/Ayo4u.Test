namespace GFA.Medicals.Infrastructure.Queries;

public class ValueCodeTypeSearchParameters : SearchParameters<int>
{
    public string[]? CodeTypes { get; set; }

    public string[]? Codes { get; set; }
}
