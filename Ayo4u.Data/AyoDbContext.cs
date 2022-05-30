using Microsoft.EntityFrameworkCore;

namespace Ayo4u.Data
{
    internal class AyoDbContext : DbContext
    {
        public AyoDbContext(DbContextOptions<AyoDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var ayoConnection = Environment.GetEnvironmentVariable("ConnectionStringsAyoContext");
                // var ayoConnection = "Server=.\\osl_233;Database=fms-local-dev;Trusted_Connection=True;MultipleActiveResultSets=true";

                if (!string.IsNullOrEmpty(ayoConnection))
                {
                    optionsBuilder.UseNpgsql(ayoConnection);
                }
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
