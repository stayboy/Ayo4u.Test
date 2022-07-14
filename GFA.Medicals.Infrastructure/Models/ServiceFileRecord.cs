namespace GFA.Medicals.Infrastructure.Models;

public class ServiceFileRecord : BaseEntity<int>
{
    public string FileTitle { get; set; } = default!;

    public string ContentType { get; set; } = default!;

    public string PathUri { get; set; } = default!;

    public string? Remarks { get; set; }

    public DateTime DateUploaded { get; set; }
}
