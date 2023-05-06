using GitHubFeatured.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GithubFeatured.Infra.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Repo> Repos { get; set; }
    }
}

