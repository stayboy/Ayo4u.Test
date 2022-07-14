namespace GFA.Medicals.Infrastructure.Queries;

public class MemberClubSearchParameters : SearchParameters<int>
{
    public int? CoachId { get; set; }

    public int? PlayerId { get; set; }

    public bool? Exited { get; set; }

    public DateTime? StartJoinDate { get; set; }

    public DateTime? EndJoinDate { get; set; }

    public DateTime? StartExitDate { get; set; }

    public DateTime? EndExitDate { get; set; }
}
