using Server.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Shared.Configurations;

public abstract class IdModelConfiguration<T, U, UModel> : IEntityTypeConfiguration<U> where UModel : IUserProfile<Guid> where U : IdModel<T, U, UModel>
{
    private readonly string tableName;
    private readonly bool excludeFromMigration;
    private readonly string schemaName;

    public IdModelConfiguration(string tableName, bool excludeFromMigration = false, string schemaName = "ayo")
    {
        this.tableName = tableName;
        this.excludeFromMigration = excludeFromMigration;
        this.schemaName = schemaName;
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
        
        if (excludeFromMigration)
        {
            builder.ToTable(tableName, ex => ex.ExcludeFromMigrations());
        }
        else
        {
            builder.ToTable(tableName, schemaName);
        }
    }
}
