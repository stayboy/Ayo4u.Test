namespace GFA.Medicals.Data.Configurations;

internal class MemberClubConfiguration : GFAIdModelConfiguration<int, MemberClub>
{
    public MemberClubConfiguration() : base("PlayerClubs")
    {
    }

    public override void Configure(EntityTypeBuilder<MemberClub> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => new { x.DateJoined, x.Club }).IsUnique();

        builder.HasOne(x => x.SigningCoach)
            .WithMany()
            .HasForeignKey(x => x.SigningCoachId);

        builder.HasOne(x => x.SellingCoach)
            .WithMany()
            .HasForeignKey(x => x.SellingCoachId);
    }
}
