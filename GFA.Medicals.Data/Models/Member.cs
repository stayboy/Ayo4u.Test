namespace GFA.Medicals.Data.Models;

internal class Member : GFAIdModel<int, Member>
{
    public string MemberType { get; set; } = default!;

    public string? GFAUniqueNo { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = default!;

    public string? Remarks { get; set; }

    public string? PictureUrl { get; set; }

    public DateTime? DateRegistered { get; set; }

    public int? PicturePathId { get; set; }

    public FileRecord? PicturePath { get; set; }

    public virtual ICollection<MemberClub> ClubRegisterations { get; set; } = new HashSet<MemberClub>();

    public virtual IReadOnlyCollection<Club> Clubs { get; set; } = new HashSet<Club>();

    public virtual ICollection<Medical> ReporterMedicalNotes { get; set; } = new HashSet<Medical>();
}
