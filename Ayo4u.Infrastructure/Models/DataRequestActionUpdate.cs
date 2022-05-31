namespace Ayo4u.Infrastructure.Models;

public record DataRequestActionUpdate (int Id, DateTime CreatedAt, bool IsSuccess = true)
{
    public ServiceAyoUser? AyoUser { get; set; }

    public Guid? RequestAyoUserId { get; set; }

    public int? ConversionId { get; set; }

    public string InputType { get; set; } = default!;

    public string OutputType { get; set; } = default!;

    public float InputValue { get; set; }

    public float? OutputValue { get; set; }

    public string? Remarks { get; set; }
}
