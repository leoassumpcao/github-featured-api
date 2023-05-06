using GitHubFeatured.Domain.Entities;

namespace GithubFeatured.Infra.Interfaces
{
    public interface IRepoRepository : IRepository<Repo>
    {
        public Task<Repo> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
