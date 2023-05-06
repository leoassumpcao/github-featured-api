using GithubFeatured.Application.Interfaces;
using GithubFeatured.Application.Models;
using GitHubFeatured.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace github_featured_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepoController : ControllerBase
    {
        private readonly IRepoService _repoService;

        public RepoController(IRepoService repoService)
        {
            _repoService = repoService;
        }


        [HttpPost(Name = "RefreshLanguageRepositories")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(
            Summary = "Refresh the most starred repositories of each language.",
            Description = "Clear all saved repositories then search for the most starred repositories of each language and save into db.")]
        public async Task<IActionResult> RefreshLanguageRepositories(CancellationToken cancellationToken)
        {
            await _repoService.RefreshAllAsync(cancellationToken);

            return NoContent();
        }

        [HttpGet(Name = "GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<RepoModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(
            Summary = "Returns starred repositories of each language that are saved in db.",
            Description = "Returns starred repositories of each language that are saved in db.")]
        public async Task<IActionResult> GetAllRepositories(
            [FromQuery] GetAllRepoFilterModel paginationFilter,
            CancellationToken cancellationToken
        )
        {
            return Ok(
                await _repoService.GetAllAsync(paginationFilter, cancellationToken));
        }

        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RepoModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [SwaggerOperation(
            Summary = "Return a specific starred repository that's saved in db.",
            Description = "Return a specific starred repository that's saved in db.")]
        public async Task<IActionResult> GetRepositoryById(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var result = await _repoService.GetByIdAsync(id, cancellationToken);
            return result is null
                ? NotFound()
                : Ok(result);
        }
    }
}