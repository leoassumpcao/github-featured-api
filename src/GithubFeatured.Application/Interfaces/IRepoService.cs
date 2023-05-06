using GithubFeatured.Application.Models;
using GitHubFeatured.Domain.Models;

namespace GithubFeatured.Application.Interfaces
{
    public interface IRepoService
    {
        public Task RefreshAllAsync(CancellationToken cancellationToken);
        public Task<PagedResponse<RepoModel>> GetAllAsync(GetAllRepoFilterModel paginationFilter, CancellationToken cancellationToken);
        public Task<RepoModel> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
