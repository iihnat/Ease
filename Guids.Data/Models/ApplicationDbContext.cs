using Microsoft.EntityFrameworkCore;

namespace Guids.Data.Models;

public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    static ApplicationDbContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    public required DbSet<GuidMetadata> Guids { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GuidMetadata>()
            .ToTable("guid_metadata") // Set custom table name
            .HasIndex(g => g.Guid)
            .IsUnique();

        modelBuilder.Entity<GuidMetadata>()
            .Property(g => g.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd(); // Specify that Id is database-generated

        modelBuilder.Entity<GuidMetadata>()
            .Property(g => g.Guid)
            .HasColumnName("guid")
            .HasMaxLength(32);

        modelBuilder.Entity<GuidMetadata>()
            .Property(g => g.User)
            .HasColumnName("user")
            .HasMaxLength(100);

        modelBuilder.Entity<GuidMetadata>()
            .Property(g => g.Expires)
            .HasColumnName("expires");

        modelBuilder.Entity<GuidMetadata>()
            .Property(g => g.CreatedDate)
            .HasColumnName("created_date");

        modelBuilder.Entity<GuidMetadata>()
            .Property(g => g.UpdatedDate)
            .HasColumnName("updated_date");
    }
}