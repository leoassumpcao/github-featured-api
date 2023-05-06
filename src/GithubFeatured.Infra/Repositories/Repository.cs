using GithubFeatured.Infra.Contexts;
using GitHubFeatured.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GithubFeatured.Infra.Interfaces
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>().AsQueryable<T>();
        }

        public async Task<IEnumerable<T>> GetAllPaginatedAsync(
            IQueryable<T> query,
            PaginationFilter paginationFilter,
            CancellationToken cancellationToken)
        {
            return await query
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllPaginatedAsync(PaginationFilter paginationFilter, CancellationToken cancellationToken)
        {
            return await _context.Set<T>()
                .Skip(paginationFilter.PageNumber * (paginationFilter.PageSize - 1))
                .Take(paginationFilter.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task ClearAsync(CancellationToken cancellationToken)
        {
            _context.Set<T>().RemoveRange(_context.Set<T>());

            await Task.CompletedTask;
        }
    }
}
