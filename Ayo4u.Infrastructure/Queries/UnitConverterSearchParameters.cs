namespace Ayo4u.Infrastructure.Queries;

public class UnitConverterSearchParameters
{
    public int[]? Ids { get; set; }

    public string? SearchText { get; set; }

    public string? InUnitType { get; set; }

    public string? OutUnitType { get; set; }

    public int? IncludeTopLogs { get; set; }

    public bool IsDeleted { get; set; } = false;
}
