namespace Ayo4u.Server.Shared.Models;

public class BaseEntity<T> where T : struct
{
    public T Id { get; set; }

    public DateTime Created { get; set; }

    public bool IsDeleted { get; set; }

    public string? CreatedByUserFullName { get; set; }

    public string? CreatedByUserEmail { get; set; }
}
