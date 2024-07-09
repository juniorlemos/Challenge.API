using Coding.Challenge.Dependencies.DataAccess.Interfaces;
using Coding.Challenge.Dependencies.Models;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using System.Linq;

namespace Coding.Challenge.Dependencies.DataAccess.Repositories;
public class ContentRepository : IContentWriteOnlyRepository, IContentReadOnlyRepository
{
    private readonly ChallengeDbContext _context;

    public ContentRepository (ChallengeDbContext context) => _context = context;

    public async Task<Content?> CreateContent(Content entity)
    {
        await _context.AddAsync(entity);
        return entity;
    }

    public async Task<Guid> Delete(Guid id)
    {
        var content = await _context.FindAsync<Content>(id);
        _context.Remove(content);
        return id;
    }

    public async Task<Content?> GetContentById(Guid id)
    {
      return await _context.FindAsync<Content>(id);        
    }

  
    public async Task<IEnumerable<Content?>> GetManyContents()
    {
        return await _context.Contents.AsNoTracking().ToListAsync();
    }

    public async Task<Content?> UpdateContent(Guid id, Content newContent)
    {
        var content = await _context.FindAsync<Content>(id);
        _context.Entry(content).CurrentValues.SetValues(newContent);
        return content;
    }
}
