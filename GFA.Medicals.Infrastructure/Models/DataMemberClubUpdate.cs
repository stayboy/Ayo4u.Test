namespace GFA.Medicals.Infrastructure.Models;

public record DataMemberClubUpdate : DataUpdate<int>
{
    public DataMemberClubUpdate(int entityId, DateTime createdAt, int clubId, DateTime dateJoined) : base(entityId, createdAt)
    {
        ClubId = clubId;
        DateJoined = dateJoined;
    }

    public int ClubId { get; set; }
    
    public string? UniqueNo { get; set; }

    public DateTime DateJoined { get; set; }

    public DateTime DateExited { get; set; }

    public int? ModeOfExit { get; set; }

    public string? Remarks { get; set; }

    public int? SigningCoachId { get; set; }

    public int? SellingCoachId { get; set; }
}