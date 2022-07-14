namespace GFA.Medicals.Data.Configurations;

internal class MedicalConfiguration : GFAIdModelConfiguration<int, Medical>
{
    public MedicalConfiguration() : base("Medicals")
    {
    }

    public override void Configure(EntityTypeBuilder<Medical> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.PlayerClub)
            .WithMany(x => x.Medicals)
            .HasForeignKey(x => x.PlayerClubId);

        builder.HasOne(x => x.ReportedBy)
            .WithMany(j => j.ReporterMedicalNotes)
            .HasForeignKey(x => x.ReportedById);

    }
}
