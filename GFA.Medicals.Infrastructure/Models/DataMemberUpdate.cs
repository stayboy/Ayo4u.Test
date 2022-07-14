namespace GFA.Medicals.Infrastructure.Models;

public record DataMemberUpdate : DataUpdate<int>
{
    public DataMemberUpdate(int EntityId, DateTime CreatedAt) : base(EntityId, CreatedAt)
    {
    }

    public DataMemberInfo? Profile { get; set; }

    public IEnumerable<DataMemberClubUpdate>? Clubs { get; set; }

    public IEnumerable<DataMedicalUpdate>? Medicals { get; set; }

    public DataFileRecordUpdate? Upload { get; set; }
}

public record DataMemberInfo (string MemberType, string FirstName, string LastName, DateTime DateOfBirth, string Gender)
{
    public string? GFAUniqueNo { get; set; }

    public string? Remarks { get; set; }

    public DateTime? DateRegistered { get; set; }

    //public string? PictureUrl { get; set; }

    //public int? PicturePathId { get; set; }
}
