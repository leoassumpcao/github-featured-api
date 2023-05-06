using GithubFeatured.Infra.Contexts;
using GithubFeatured.Infra.Interfaces;
using GithubFeatured.Infra.Services.GitHub;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GithubFeatured.Infra
{
    public static class Dependencies
    {
        public static void RegisterInfraServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                x => x.UseSqlServer(configuration.GetConnectionString("AppDb")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRepoRepository, RepoRepository>();

            services.AddScoped<IGitHubApiService, GitHubApiService>();
        }
    }
}
