using Coding.Challenge.Dependencies.DataAccess.Interfaces;
using Coding.Challenge.Dependencies.Database;
using Coding.Challenge.Dependencies.Models;

namespace Coding.Challenge.Dependencies.Managers
{
    public class ContentsManager : IContentsManager
    {
        private readonly IContentWriteOnlyRepository _contentWriteOnlyRepository;
        private readonly IContentReadOnlyRepository _contentReadOnlyRepository;
        private readonly IMapper<Content, ContentDto> _contentMapper;

        public ContentsManager(IContentWriteOnlyRepository contentWriteOnlyRepository,
                               IContentReadOnlyRepository contentReadOnlyRepository,
                               IMapper<Content, ContentDto> contentMapper)
        {
            _contentWriteOnlyRepository = contentWriteOnlyRepository;
            _contentReadOnlyRepository = contentReadOnlyRepository;
            _contentMapper = contentMapper;

        }

        public async Task<IEnumerable<Content?>> GetManyContents()
        {
            return await _contentReadOnlyRepository.GetManyContents();
        }

        public async Task<IEnumerable<Content?>> GetFilteredContents(string? title, string? genre)
        {
            var contents = await _contentReadOnlyRepository.GetManyContents();

            if (!string.IsNullOrEmpty(title))
            {
                contents = contents.Where(c => c?.Title.Contains(title, StringComparison.OrdinalIgnoreCase) ?? false);
            }

            if (!string.IsNullOrEmpty(genre))
            {
                contents = contents.Where(c => c?.GenreList.Any(g => g.Contains(genre, StringComparison.OrdinalIgnoreCase)) ?? false);
            }

            return contents;
        }

        public async Task<Content?> CreateContent(ContentDto contentDto)
        {
            var newContentId = Guid.NewGuid();
            var content = _contentMapper.Map(newContentId, contentDto);

            return await _contentWriteOnlyRepository.CreateContent(content);               
                
        }

        public async Task<Content?> GetContent(Guid id)
        {
            return await _contentReadOnlyRepository.GetContentById(id);
        }

        public async Task<Content?> UpdateContent(Guid id, ContentDto newContent)
        {
            var oldContent = await _contentReadOnlyRepository.GetContentById(id);

            var content = _contentMapper.Patch(oldContent, newContent);

            return await _contentWriteOnlyRepository.UpdateContent(id, content);
        }

        public async Task<Guid> DeleteContent(Guid id)
        {
            return await _contentWriteOnlyRepository.Delete(id);
        }

        public async Task<bool> AddGenres(Guid id, IEnumerable<string> genres)
        {
            var content = await _contentReadOnlyRepository.GetContentById(id);

            if (content == null)
                return false;

            var updatedGenres = content.GenreList.Concat(genres).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            var updatedContentDto = new ContentDto(
                content.Title,
                content.SubTitle,
                content.Description,
                content.ImageUrl,
                content.Duration,
                content.StartTime,
                content.EndTime,
                updatedGenres
            );
            var newContent = _contentMapper.Patch(content, updatedContentDto);
            var result = await _contentWriteOnlyRepository.UpdateContent(id, newContent);
            return result != null;
        }

        public async Task<bool> RemoveGenres(Guid id, IEnumerable<string> genres)
        {
            var content = await _contentReadOnlyRepository.GetContentById(id);

            if (content == null)
                return false;

            var updatedGenres = content.GenreList.Except(genres, StringComparer.OrdinalIgnoreCase).ToList();
            var updatedContentDto = new ContentDto(
                content.Title,
                content.SubTitle,
                content.Description,
                content.ImageUrl,
                content.Duration,
                content.StartTime,
                content.EndTime,
                updatedGenres
            );

            var newContent = _contentMapper.Patch(content, updatedContentDto);
            var result = await _contentWriteOnlyRepository.UpdateContent(id, newContent);
            return result != null;
        }
    }
}
