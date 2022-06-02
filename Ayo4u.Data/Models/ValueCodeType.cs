namespace Ayo4u.Data.Models;

internal class ValueCodeType : IdModel<int, ValueCodeType>
{
    public string CodeType { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}
