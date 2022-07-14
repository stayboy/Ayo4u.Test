namespace GFA.Medicals.Infrastructure.Models;

public record DataFileRecordUpdate : DataUpdate<int>
{
    public DataFileRecordUpdate(int entityId, DateTime createdAt) : base(entityId, createdAt)
    {
    }

    public FileRecordInfo? Record { get; set; }

    public string? PathUri { get; set; }
}

public record FileRecordInfo(string Title, string ContentType)
{
    public string? Remarks { get; set; }
}
