using Coding.Challenge.Dependencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding.Challenge.Dependencies.Managers
{
    public interface IContentsManager
    {
        Task<IEnumerable<Content?>> GetManyContents();
        Task<IEnumerable<Content?>> GetFilteredContents(string? title, string? genre);
        Task<Content?> CreateContent(ContentDto content);
        Task<Content?> GetContent(Guid id);
        Task<Content?> UpdateContent(Guid id, ContentDto content);
        Task<Guid> DeleteContent(Guid id);
        Task<bool> AddGenres(Guid id, IEnumerable<string> genres);
        Task<bool> RemoveGenres(Guid id, IEnumerable<string> genres);
    }
}
