using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayo4u.Data.Configurations
{
    internal class RequestActionConfiguration : AyoIdModelConfiguration<int, RequestAction>
    {
        public RequestActionConfiguration() : base("RequestLogs")
        {
        }

        public override void Configure(EntityTypeBuilder<RequestAction> builder)
        {
            base.Configure(builder);

            builder.HasOne(q => q.RequestAyoUser)
                .WithMany()
                .HasForeignKey(q => q.RequestAyoUserId);

            builder.HasOne(q => q.Conversion)
                .WithMany(q => q.RequestLogs)
                .HasForeignKey(q => q.ConversionId);
        }
    }
}
