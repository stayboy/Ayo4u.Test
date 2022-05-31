namespace Ayo4u.Web.Shared.Queries;

public class ApiUserSearchParameters
{
    public Guid[]? UserIds { get; set; }

    public string? email { get; set; }

    public string? name { get; set; }

    public bool deleted { get; set; } = false;

    public DateTime? fromCreatedDate { get; set; }

    public DateTime? toCreatedDate { get; set; }
}
