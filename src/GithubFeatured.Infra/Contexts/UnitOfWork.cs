using GithubFeatured.Infra.Interfaces;

namespace GithubFeatured.Infra.Contexts
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> CommitAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
