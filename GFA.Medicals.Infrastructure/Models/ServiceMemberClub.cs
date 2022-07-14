namespace GFA.Medicals.Infrastructure.Models;

public class ServiceMemberClub : BaseEntity<int>
{
    public int PlayerId { get; set; }

    public int ClubId { get; set; }

    public string? UniqueNo { get; set; }

    public DateTime DateJoined { get; set; }

    public DateTime? DateExited { get; set; }

    public int? ModeOfExit { get; set; }

    public string? Remarks { get; set; }

    public int? SigningCoachId { get; set; }

    public int? SellingCoachId { get; set; }

    public ServiceClub? Club { get; set; }

    public ServiceMember? Player { get; set; }

    public ServiceMember? SigningCoach { get; set; }

    public ServiceMember? SellingCoach { get; set; }

    public IEnumerable<ServiceMedical>? Medicals { get; set; }
}
