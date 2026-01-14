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
    public DbSet<DynamicFormField> DynamicFormFields { get; set; }





    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<FormSummary>()
    //        .HasNoKey()          // 👈 REQUIRED
    //        .ToView(null);       // 👈 IMPORTANT: tells EF this is NOT a table
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FormSummary>().HasNoKey();
        modelBuilder.Entity<DynamicFormField>().HasNoKey(); // Add this line!

        // Configure DynamicFormField explicitly
        modelBuilder.Entity<DynamicFormField>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Name).HasColumnName("Name");
            entity.Property(e => e.Label).HasColumnName("Label");
            entity.Property(e => e.Type).HasColumnName("Type");
            entity.Property(e => e.Required).HasColumnName("Required");
        });
    }


}
