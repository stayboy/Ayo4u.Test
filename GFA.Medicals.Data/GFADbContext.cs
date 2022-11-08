using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GFA.Medicals.Data;

internal class GFADbContext : IdentityDbContext<GFAIdentityUser>
{
    public GFADbContext(DbContextOptions<GFADbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var ayoConnection = Environment.GetEnvironmentVariable("ConnectionStringsGFAContext");
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

internal class GFAIdentityUser : IdentityUser<GFAUser>
{
}