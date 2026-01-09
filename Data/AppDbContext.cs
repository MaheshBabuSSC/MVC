using Microsoft.EntityFrameworkCore;
using MvcWebApiSwaggerApp.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<DbTest> DbTest { get; set; }
    public DbSet<FormSummary> FormSummaries { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FormSummary>()
            .HasNoKey()          // 👈 REQUIRED
            .ToView(null);       // 👈 IMPORTANT: tells EF this is NOT a table
    }

}
