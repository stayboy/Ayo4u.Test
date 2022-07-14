namespace GFA.Medicals.Web.Shared.Models;

public record ApiValueCodeTypeChange : ApiChange<int>
{
    public ApiValueCodeTypeChange(int Id, DateTime? Created = null) : base(Id, Created)
    {
    }

    public string CodeType { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}
