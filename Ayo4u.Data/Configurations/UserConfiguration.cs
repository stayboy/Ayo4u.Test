using Ayo4u.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ayo4u.Data.Configurations
{
    internal class UserConfiguration : IdModelConfiguration<Guid, AyoUser>
    {
        public UserConfiguration() : base("AyoUsers")
        {
        }

        public override void Configure(EntityTypeBuilder<AyoUser> builder)
        {
            base.Configure(builder);

            builder.HasData(new AyoUser[] { new() { Email = "skwart@outlook.com", FirstName = "Solomon", LastName = "Owoo" } });
        }
    }
}
