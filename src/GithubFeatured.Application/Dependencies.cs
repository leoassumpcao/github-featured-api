using GithubFeatured.Application.Interfaces;
using GithubFeatured.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace GithubFeatured.Application
{
    [ExcludeFromCodeCoverage]
    public static class Dependencies
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRepoService, RepoService>();
        }
    }
}
