namespace Ayo4u.Web.Shared.Models;

public class ApiRequestAction : ApiBaseEntity<int>
{
    public ApiAyoUser RequestAyoUser { get; set; } = default!;

    public int? ConversionId { get; set; }

    // public ApiUnitConverter? Conversion { get; set; }

    public bool IsSuccess { get; set; }

    public string InUnitType { get; set; } = default!;

    public string OutUnitType { get; set; } = default!;

    public float InputValue { get; set; }

    public float? OutputValue { get; set; }

    public string? Remarks { get; set; }
}
