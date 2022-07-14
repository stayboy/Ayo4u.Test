using Microsoft.EntityFrameworkCore;

namespace GFA.Medicals.Data.Configurations
{
    internal abstract class GFAIdModelConfiguration<T, U> : IdModelConfiguration<T, U, GFAUser> where U : GFAIdModel<T, U>
    {
        protected GFAIdModelConfiguration(string tableName, bool excludeFromMigration = false) : base(tableName, excludeFromMigration, "gfa")
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
}
