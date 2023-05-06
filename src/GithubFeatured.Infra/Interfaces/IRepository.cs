using GitHubFeatured.Domain.Models;

namespace GithubFeatured.Infra.Interfaces
{
    public interface IRepository<T>
    {
        public IQueryable<T> GetQueryable();
        public Task<IEnumerable<T>> GetAllPaginatedAsync(IQueryable<T> query, PaginationFilter paginationFilter, CancellationToken cancellationToken);
        public Task<IEnumerable<T>> GetAllPaginatedAsync(PaginationFilter paginationFilter, CancellationToken cancellationToken);
        public Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        public Task ClearAsync(CancellationToken cancellationToken);
    }
}
