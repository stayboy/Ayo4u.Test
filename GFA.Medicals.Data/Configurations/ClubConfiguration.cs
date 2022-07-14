using GFA.Medicals.Data.Configurations;
namespace GFA.Medicals.Data.Configurations;

internal class ClubConfiguration : GFAIdModelConfiguration<int, Club>
{
    public ClubConfiguration() : base("Clubs")
    {       
    }

    public override void Configure(EntityTypeBuilder<Club> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.ClubCountry)
            .WithMany()
            .HasForeignKey(x => x.CountryId);

        builder.HasMany(x => x.Players)
            .WithMany(j => j.Clubs)
            .UsingEntity<MemberClub>();            
    }
}
