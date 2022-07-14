namespace GFA.Medicals.Data.Configurations;

internal class MemberConfiguration : GFAIdModelConfiguration<int, Member>
{
    public MemberConfiguration() : base("Members")
    {
    }

    public override void Configure(EntityTypeBuilder<Member> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.ClubRegisterations)
            .WithOne(x => x.Player)
            .HasForeignKey(x => x.PlayerId);
    }
}
