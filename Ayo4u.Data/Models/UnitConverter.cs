namespace Ayo4u.Data.Models;

internal class UnitConverter : AyoIdModel<int, UnitConverter>
{
    public string InUnitType { get; set; } = default!;

    public string OutUnitType { get; set; } = default!;

    public float Multiplier { get; set; }

    public Formulae? Formula { get; set; }

    public virtual IReadOnlyCollection<RequestAction> RequestLogs { get; set; } = new HashSet<RequestAction>();
}
