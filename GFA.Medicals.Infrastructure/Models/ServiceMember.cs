namespace GFA.Medicals.Infrastructure.Models;

public class ServiceMember : BaseEntity<int>
{
    public string MemberType { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = default!;

    public string? Remarks { get; set; }

    public string? PictureUrl { get; set; }

    public DateTime? DateRegistered { get; set; }

    public int? PicturePathId { get; set; }

    public ServiceFileRecord? PicturePath { get; set; }

    public IEnumerable<ServiceMemberClub>? ClubRegisterations { get; set; }

    public IEnumerable<ServiceClub>? ClubsPlayed { get; set; }

    public IEnumerable<ServiceMedical>? ReporterMedicalNotes { get; set; }

    public IEnumerable<ServiceMedical>? PlayerMedicalNotes { get; set; }
}
