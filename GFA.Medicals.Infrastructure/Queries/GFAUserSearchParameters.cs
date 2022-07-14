namespace GFA.Medicals.Infrastructure.Queries;

public class GFAUserSearchParameters : SearchParameters<Guid>
{
    public string? Email { get; set; }

    public string? LastName { get; set; }
}
