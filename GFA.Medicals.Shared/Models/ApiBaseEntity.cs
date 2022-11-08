namespace GFA.Medicals.Web.Shared.Models;

public abstract class ApiBaseEntity
{
    public DateTime Created { get; set; }

    public bool IsDeleted { get; set; } = false;

    public string? CreatedByUserFullName { get; set; }

    public string? CreatedByUserEmail { get; set; }
}

public abstract class ApiBaseEntity<T> : ApiBaseEntity
{
    public T Id { get; set; } = default!;
}
