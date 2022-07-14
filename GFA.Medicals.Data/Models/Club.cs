namespace GFA.Medicals.Data.Models;

internal class Club : GFAIdModel<int, Club>
{
    public int CountryId { get; set; }

    public string Name { get; set; } = default!;

    public string? Initials { get; set; }

    public string? Division { get; set; }

    public DateTime? Effective { get; set; }

    public ValueCodeType? ClubCountry { get; set; }

    public virtual IReadOnlyCollection<Member>? Players { get; set; }
}
