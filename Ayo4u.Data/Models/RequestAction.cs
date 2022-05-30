namespace Ayo4u.Data.Models;

internal class RequestAction : IdModel<int, RequestAction>
{
    public Guid RequestAyoUserId { get; set; }

    public int ConversionId { get; set; }

    public float InputValue { get; set; }

    public float? OutputValue { get; set; }

    public bool IsSuccess { get; set; } = true;

    public string? LogMessage { get; set; }

    public UnitConverter? Conversion { get; set; }

    public AyoUser RequestAyoUser { get; set; } = default!;
}
