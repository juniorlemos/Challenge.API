using Coding.Challenge.Dependencies.Models;
using Microsoft.EntityFrameworkCore;

namespace Coding.Challenge.insfrastructure.DataAccess;
public class ChallengeDbContext : DbContext
{
    public ChallengeDbContext (DbContextOptions options) : base(options) { }

    public DbSet<Content> Contents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChallengeDbContext).Assembly);
    }
}
