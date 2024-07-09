using Coding.Challenge.API.Models;
using Coding.Challenge.Dependencies.DataAccess.Interfaces;
using Coding.Challenge.Dependencies.Database.Interfaces;
using Coding.Challenge.Dependencies.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Coding.Challenge.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContentController : Controller
    {
        private readonly IContentsManager _manager;
        private readonly ILoggerService _logger;
        private readonly IUnityOfWork _unityOfWork;
        public ContentController(IContentsManager manager, ILoggerService logger, IUnityOfWork unityOfWork)
        {
            _manager = manager;
            _logger = logger;
            _unityOfWork = unityOfWork;
        }

        [HttpGet]
        [Obsolete("This endpoint is deprecated. Use GET /api/v1/Content/filter instead.")]
        public async Task<IActionResult> GetManyContents()
        {
            var contents = await _manager.GetManyContents();

            if (!contents.Any())
            {
                //await _logger.LogAsync("GetManyContents", "No contents found");
                return NotFound();
            }
            //await _logger.LogAsync("GetManyContents", "Getting all contents");

            return Ok(contents);
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredContents([FromQuery] string? title, [FromQuery] string? genre)
        {
            var contents = await _manager.GetFilteredContents(title, genre);

            if (!contents.Any())
            {
                //await _logger.LogAsync("GetFilteredContents", "No contents found with the given filters").ConfigureAwait(false);
                return NotFound();
            }

            //await _logger.LogAsync("GetFilteredContents", $"Filtering contents by title: {title} and genre: {genre}").ConfigureAwait(false);
            return Ok(contents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContent(Guid id)
        {
            var content = await _manager.GetContent(id);

            if (content == null)
            {
                //await _logger.LogAsync("GetContent", $"Content with id {id} not found");
                return NotFound();
            }

            //await _logger.LogAsync("GetContent", $"Getting content with id {id}");
            return Ok(content);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContent([FromBody] ContentInput content)
        {
            var createdContent = await _manager.CreateContent(content.ToDto());

            if (createdContent == null)
            {
                //await _logger.LogAsync("CreateContent", "Failed to create content");
                return Problem();
            }

            //await _logger.LogAsync("CreateContent", $"Content created with id {createdContent.Id}");
            await _unityOfWork.Commit();
            return createdContent == null ? Problem() : Ok(createdContent);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateContent(Guid id, [FromBody] ContentInput content)
        {
            var updatedContent = await _manager.UpdateContent(id, content.ToDto());

            if (updatedContent == null)
            {
                //await _logger.LogAsync("UpdateContent", $"Content with id {id} not found");
                return NotFound();
            }

            //await _logger.LogAsync("UpdateContent", $"Content with id {id} updated");
            await _unityOfWork.Commit();
            return updatedContent == null ? NotFound() : Ok(updatedContent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(Guid id)
        {
            var deletedId = await _manager.DeleteContent(id);

            //await _logger.LogAsync("DeleteContent", $"Content with id {id} deleted");
            await _unityOfWork.Commit();
            return Ok(deletedId);
        }

        [HttpPost("{id}/genre")]
        public async Task<IActionResult> AddGenres(Guid id, [FromBody] IEnumerable<string> genres)
        {
            var result = await _manager.AddGenres(id, genres);

            if (!result)
            {
                //await _logger.LogAsync("AddGenres", $"Failed to add genres to content with id {id}");
                return Problem();
            }

            //await _logger.LogAsync("AddGenres", $"Genres added to content with id {id}");
            await _unityOfWork.Commit();
            return result ? Ok() : Problem();
        }

        [HttpDelete("{id}/genre")]
        public async Task<IActionResult> RemoveGenres(Guid id, [FromBody] IEnumerable<string> genres)
        {
            var result = await _manager.RemoveGenres(id, genres);

            if (!result)
            {
                //await _logger.LogAsync("RemoveGenres", $"Failed to remove genres from content with id {id}");
                return Problem();
            }

            //await _logger.LogAsync("RemoveGenres", $"Genres removed from content with id {id}");
            await _unityOfWork.Commit();
            return result ? Ok() : Problem();
        }
    }
}
