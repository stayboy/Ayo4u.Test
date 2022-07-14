namespace Ayo4u.Data.Configurations;

internal class AyoIdModelConfiguration<T, U> : IdModelConfiguration<T, U, AyoUser> where U : AyoIdModel<T, U>
{
    public AyoIdModelConfiguration(string tableName, bool excludeFromMigration = false) : base(tableName, excludeFromMigration, "ayo")
    {
    }

    public override void Configure(EntityTypeBuilder<U> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ModifiedByUser)
            .WithMany()
            .HasForeignKey(x => x.ModifiedByUserId).OnDelete(DeleteBehavior.NoAction);
    }
}
