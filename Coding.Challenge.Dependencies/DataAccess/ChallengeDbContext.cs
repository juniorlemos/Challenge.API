using Coding.Challenge.Dependencies.Models;
using Microsoft.EntityFrameworkCore;

namespace Coding.Challenge.Dependencies.DataAccess;
public class ChallengeDbContext : DbContext
{
    public ChallengeDbContext (DbContextOptions options) : base(options) { }

    public DbSet<Content> Contents { get; set; }

  
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Content>(entity =>
        {
            entity.ToTable("Contents");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.SubTitle);
            entity.Property(e => e.Description);
            entity.Property(e => e.ImageUrl);
            entity.Property(e => e.Duration);
            entity.Property(e => e.StartTime);
            entity.Property(e => e.EndTime);
            entity.Property(e => e.GenreList)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        });
    }
}
