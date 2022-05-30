using Ayo4u.Server.Shared.Models;
namespace Ayo4u.Infrastructure.Models;

public class ServiceRequestAction : BaseEntity<int>
{
    public ServiceAyoUser RequestAyoUser { get; set; } = default!;

    public int ConversionId { get; set; }

    public ServiceUnitConverter? Conversion { get; set; }

    public bool IsSuccess { get; set; }

    public string? InUnitType { get; set; } = default!;

    public string? OutUnitType { get; set; } = default!;

    public float InputValue { get; set; }

    public float? OutputValue { get; set; }

    public string? Remarks { get; set; }
}
