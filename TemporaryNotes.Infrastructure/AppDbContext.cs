using Microsoft.EntityFrameworkCore;
using TemporaryNotes.Domain.Entities;

namespace TemporaryNotes.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Notes> Notes => Set<Notes>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notes>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Code)
                .IsUnique();

            entity.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.PasswordHash)
                .HasMaxLength(255);

            entity.Property(x => x.ViewCount)
                .HasDefaultValue(0);

            entity.ToTable("Notes", table =>
            {
                table.HasCheckConstraint(
                    "CK_Notes_ViewCount_NonNegative",
                    "[ViewCount] >= 0");

                table.HasCheckConstraint(
                    "CK_Notes_MaxViews_Positive",
                    "[MaxViews] IS NULL OR [MaxViews] > 0");
            });
        });
    }
}