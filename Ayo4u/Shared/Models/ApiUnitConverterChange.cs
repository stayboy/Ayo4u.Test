namespace Ayo4u.Web.Shared.Models;

public record ApiUnitConverterChange : ApiChange<int>
{
    public ApiUnitConverterChange(int Id, DateTime? Created = null) : base(Id, Created)
    {
    }

    public string InUnitType { get; set; } = default!;

    public string OutUnitType { get; set; } = default!;

    public float Multiplier { get; set; }

    public int? Formula { get; set; }
}
