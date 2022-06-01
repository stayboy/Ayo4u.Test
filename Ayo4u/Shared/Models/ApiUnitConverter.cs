namespace Ayo4u.Web.Shared.Models;

public class ApiUnitConverter : ApiBaseEntity<int>
{
    public string InUnitType { get; set; } = default!;

    public string OutUnitType { get; set; } = default!;

    public float Multiplier { get; set; }

    public ApiValueTypeRecord<int>? Formula { get; set; }

    public IEnumerable<ApiRequestAction>? RequestLogs { get; set; }
}
