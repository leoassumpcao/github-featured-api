using GithubFeatured.Infra.Contexts;
using GitHubFeatured.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GithubFeatured.Infra.Interfaces
{
    public class RepoRepository : Repository<Repo>, IRepoRepository
    {
        public RepoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Repo> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
