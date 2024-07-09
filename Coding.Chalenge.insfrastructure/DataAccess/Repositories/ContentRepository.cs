using Coding.Challenge.Dependencies.Models;
using Coding.Challenge.insfrastructure.DataAccess.Interfaces;

namespace Coding.Challenge.insfrastructure.DataAccess.Repositories;
public class ContentRepository : IContentWriteOnlyRepository
{
    private readonly ChallengeDbContext _context;

    public ContentRepository (ChallengeDbContext context) => _context = context;

    public async Task<Content?> CreateContent(Content entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
