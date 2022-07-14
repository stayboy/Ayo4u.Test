namespace GFA.Medicals.Infrastructure.Models;

public class ServiceValueCodeType : BaseEntity<int>
{
    public string CodeType { get; set; } = default!;

    public string ValueCode { get; set; } = default!;

    public string ValueName { get; set; } = default!;

    public string? Description { get; set; }
}
