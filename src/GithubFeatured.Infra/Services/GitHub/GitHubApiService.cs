using GithubFeatured.Infra.Interfaces;
using GithubFeatured.Infra.Services.GitHub.Exceptions;
using GithubFeatured.Infra.Services.GitHub.Models;
using GithubFeatured.Infra.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GithubFeatured.Infra.Services.GitHub
{
    public class GitHubApiService : IGitHubApiService
    {
        private const string GITHUB_API_BASE_URL = "https://api.github.com";
        private HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GitHubApiService> _logger;

        public GitHubApiService(IConfiguration configuration, ILogger<GitHubApiService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            ConfigureClient();
        }

        public GitHubApiService(HttpClient httpClient, IConfiguration configuration, ILogger<GitHubApiService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
        }

        private void ConfigureClient()
        {
            var gitHubPat = GetGitHubPat();

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(GITHUB_API_BASE_URL),
            };
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {gitHubPat}");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", $"HttpClient");
        }

        private string GetGitHubPat()
        {
            var result = _configuration.GetSection("GitHub")?.GetSection("ApiKey")?.Value;
            return string.IsNullOrEmpty(result)
                ? throw new GithubApiException("Could not find GitHub/ApiKey on appsettings.")
                : result;
        }

        public async Task<GithubRepoSearchModel> FindTopRatedByLangAsync(
            string languageName,
            int perPage,
            int page,
            CancellationToken cancellationToken = default
        )
        {
            var queryParameters = new Dictionary<string, string>
            {
                { "q", $"language:{languageName}" },
                { "sort", "stars" },
                { "order", "desc" },
                { "per_page", perPage.ToString() },
                { "page", page.ToString() }
            };
            var requestUri = RequestUriUtil.GetUriWithQueryString("/search/repositories", queryParameters);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            _logger.LogInformation("FindTopRatedByLangAsync is starting a request for language {languageName}...", languageName);


            var response = await _httpClient.SendAsync(request, cancellationToken);
            _logger.LogInformation("Response for request {languageName}: [{statusCode}] {content}",
                languageName,
                response.StatusCode,
                await response.Content.ReadAsStringAsync());

            return response is null || response.StatusCode != System.Net.HttpStatusCode.OK
                ? throw new GithubApiException()
                : JsonConvert.DeserializeObject<GithubRepoSearchModel>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() } });
        }
    }
}
