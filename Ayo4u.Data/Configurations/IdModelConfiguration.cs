using Ayo4u.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayo4u.Data.Configurations;

internal abstract class IdModelConfiguration<T, U> : IEntityTypeConfiguration<U> where U : IdModel<T, U>
{
    private readonly string tableName;
    private readonly bool excludeFromMigration;

    public IdModelConfiguration(string tableName, bool excludeFromMigration = false)
    {
        this.tableName = tableName;
        this.excludeFromMigration = excludeFromMigration;
    }

    public virtual void Configure(EntityTypeBuilder<U> builder)
    {
        builder.HasKey(x => x.Id);

        if (typeof(U) == typeof(Guid))
        {
            builder.Property("Id").HasDefaultValueSql("(newid())");
        }

        if (typeof(U) == typeof(int))
        {
            builder.Property("Id").ValueGeneratedOnAdd();
        }

        builder.Property(x => x.IsDeleted)
            .HasDefaultValueSql("(CONVERT([bit],(0)))");

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GetUtcDate()").ValueGeneratedOnAdd();
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GetUtcDate()").ValueGeneratedOnAddOrUpdate();

        builder.Property(x => x.CreatedAt).Metadata
            .SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(x => x.CreatedByUserId).Metadata
            .SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.ModifiedByUser)
            .WithMany()
            .HasForeignKey(x => x.ModifiedByUserId).OnDelete(DeleteBehavior.NoAction);

        if (excludeFromMigration)
        {
            builder.ToTable(tableName, ex => ex.ExcludeFromMigrations());
        }
        else
        {
            builder.ToTable(tableName, "ayo");
        }
    }

}
