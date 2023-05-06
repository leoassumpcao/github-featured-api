using AutoMapper;
using GithubFeatured.Application.Interfaces;
using GithubFeatured.Application.Models;
using GithubFeatured.Infra.Interfaces;
using GitHubFeatured.Domain.Entities;
using GitHubFeatured.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GithubFeatured.Application.Services
{
    public class RepoService : IRepoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RepoService> _logger;
        private readonly IGitHubApiService _gitHubApiRepository;
        private readonly IRepoRepository _repoRepository;
        private readonly IMapper _mapper;

        public RepoService(
            IConfiguration configuration,
            ILogger<RepoService> logger,
            IGitHubApiService gitHubApiRepository,
            IRepoRepository repoRepository,
            IMapper mapper)
        {
            _configuration = configuration;
            _logger = logger;
            _gitHubApiRepository = gitHubApiRepository;
            _repoRepository = repoRepository;
            _mapper = mapper;
        }

        private IEnumerable<string> GetTrackedLanguages()
        {
            var result = _configuration.GetSection("TrackedLanguages")?.GetChildren();

            return result is null || result.Count() == 0
                ? throw new Exception("Could not find TrackedLanguages on appsettings.")
                : result.Select(r => r.Value);
        }

        public async Task RefreshAllAsync(CancellationToken cancellationToken)
        {
            await ClearRepositories(cancellationToken);

            var trackedTanguages = GetTrackedLanguages();
            _logger.LogInformation("The languages that will have repositories tracked on GitHub are: {trackedTanguages}", trackedTanguages);

            foreach (var language in trackedTanguages)
            {
                await CreateLangRepositories(language, cancellationToken);
            }

            _logger.LogInformation("Repositories have been refreshed.");
        }

        private async Task ClearRepositories(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cleaning up repositories...");

            await _repoRepository.ClearAsync(cancellationToken);

            _logger.LogInformation("Repositories have been cleaned up.");
        }

        private async Task CreateLangRepositories(string language, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for the most starred repositories of language {language}...", language);

            var gitHubTopRatedRepositories = await _gitHubApiRepository.FindTopRatedByLangAsync(language, 10, 1, cancellationToken);

            foreach (var gitHubRepository in gitHubTopRatedRepositories?.Items)
            {
                if (gitHubRepository is not null && gitHubRepository.Id != 0)
                {
                    _logger.LogInformation("Creating repository {trackedTanguages} of language {language}", gitHubRepository.Name, language);

                    var repo = _mapper.Map<Repo>(gitHubRepository);

                    await _repoRepository.AddAsync(repo, cancellationToken);
                }
            }

            _logger.LogInformation("Sucessfully created the most starred repositories of language {language}.", language);
        }

        public async Task<PagedResponse<RepoModel>> GetAllAsync(GetAllRepoFilterModel paginationFilter, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving repositories using filter: {filter}", JsonConvert.SerializeObject(paginationFilter));

            var query = GetQueryBasedOnFilter(paginationFilter);

            var pagedRepos = await _repoRepository
                .GetAllPaginatedAsync(query, paginationFilter, cancellationToken);

            _logger.LogInformation("Retrieved {pagedReposCount} repositories using filter: {filter}", pagedRepos.Count(), JsonConvert.SerializeObject(paginationFilter));

            var pagedRepoModels = _mapper.Map<IEnumerable<RepoModel>>(pagedRepos);

            return new PagedResponse<RepoModel>(
                pagedRepoModels,
                paginationFilter.PageNumber,
                paginationFilter.PageSize
            );
        }

        private IOrderedQueryable<Repo> GetQueryBasedOnFilter(GetAllRepoFilterModel paginationFilter)
        {
            var query = _repoRepository
                .GetQueryable()
                .AsNoTracking();

            if (!string.IsNullOrEmpty(paginationFilter.Language))
            {
                query = query
                    .Where(repo => repo.Language == paginationFilter.Language);
            }

            return query.OrderBy(repo => repo.Id);
        }

        public async Task<RepoModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var repo = await _repoRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<RepoModel>(repo);
        }
    }
}
