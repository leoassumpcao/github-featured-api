using GithubFeatured.Infra.Services.GitHub.Models;

namespace GithubFeatured.Infra.Interfaces
{
    public interface IGitHubApiService
    {
        public Task<GithubRepoSearchModel> FindTopRatedByLangAsync(string languageName, int perPage = 10, int page = 1, CancellationToken cancellationToken = default);
    }
}
