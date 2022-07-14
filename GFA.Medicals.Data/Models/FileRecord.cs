namespace GFA.Medicals.Data.Models;

internal class FileRecord : GFAIdModel<int, FileRecord>
{
    public string Title { get; set; } = default!;

    public string ContentType { get; set; } = default!;

    public string PathUri { get; set; } = default!;

    public DateTime Uploaded { get; set; }

    public string? Remarks { get; set; }

    public virtual IReadOnlyCollection<Member>? PictureMembers { get; set; }

    public virtual IReadOnlyCollection<Medical>? FileMedicals { get; set; }
}
