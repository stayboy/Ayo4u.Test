namespace Ayo4u.Web.Shared.Models;

public record ApiRequestChange : ApiChange<int>
{
    public ApiRequestChange(int Id, DateTime? Created = null) : base(Id, Created)
    {
    }

    public int? ConversionId { get; set; }

    public bool IsSuccess { get; set; }

    public string InUnitType { get; set; } = default!;

    public string OutUnitType { get; set; } = default!;

    public float InputValue { get; set; }

    public float? OutputValue { get; set; }

}
