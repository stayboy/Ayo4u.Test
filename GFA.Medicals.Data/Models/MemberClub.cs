namespace GFA.Medicals.Data.Models
{
    internal class MemberClub : GFAIdModel<int, MemberClub>
    {
        public int PlayerId { get; set; }

        public int ClubId { get; set; }

        public string? UniqueNo { get; set; }

        public DateTime DateJoined { get; set; }

        public DateTime? DateExited { get; set; }

        public int? ExitStatus { get; set; }

        public string? Remarks { get; set; }

        public int? SigningCoachId { get; set; }

        public int? SellingCoachId { get; set; }

        public virtual Club Club { get; set; } = default!;

        public Member? Player { get; set; }

        public Member? SigningCoach { get; set; }

        public Member? SellingCoach { get; set; }

        public virtual ICollection<Medical> Medicals { get; set; } = new HashSet<Medical>();
    }
}
