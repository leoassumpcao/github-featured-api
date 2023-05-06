using AutoMapper;
using GithubFeatured.Application.Mapping;
using GithubFeatured.Application.Services;
using GithubFeatured.Infra.Interfaces;
using GithubFeatured.Infra.Services.GitHub.Models;
using GithubFeatured.Tests.Common.Factories;
using GitHubFeatured.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace GithubFeatured.Application.Tests.Services.RepoServiceTests
{
    public class RepoServiceRefreshAsyncTests
    {
        private readonly Mock<ILogger<RepoService>> _loggerMock;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly Mock<IRepoRepository> _repoRepositoryMock;

        public RepoServiceRefreshAsyncTests()
        {
            _loggerMock = new Mock<ILogger<RepoService>>();

            _configuration = ConfigurationFactory.GenerateValid();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RepoProfile());
            });
            _mapper = mockMapper.CreateMapper();

            _repoRepositoryMock = new Mock<IRepoRepository>();
            _repoRepositoryMock.Setup(s => s.ClearAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task RefreshAsync_Success()
        {
            // Arrange
            var totalRepos = 10;
            var totalLanguages = 5;
            var configurationWithFiveLanguages = ConfigurationFactory.GenerateValid();

            var repoRepository = new Mock<IRepoRepository>();
            repoRepository.Setup(s => s.ClearAsync(It.IsAny<CancellationToken>()));

            var gitHubApiService = new Mock<IGitHubApiService>();
            var githubRepoModels = GithubRepoModelFactory.GenerateMultipleRandomValid(totalRepos);
            gitHubApiService.Setup(s => s.FindTopRatedByLangAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GithubRepoSearchModel()
                {
                    IncompleteResults = false,
                    TotalCount = githubRepoModels.Count(),
                    Items = githubRepoModels
                });

            var repoService = new RepoService(configurationWithFiveLanguages, _loggerMock.Object, gitHubApiService.Object, repoRepository.Object, _mapper);


            //Act
            var exception = await Record.ExceptionAsync(() => repoService.RefreshAllAsync(new CancellationToken()));


            //Assert
            Assert.Null(exception);
            repoRepository.Verify(repo => repo.AddAsync(It.IsAny<Repo>(), It.IsAny<CancellationToken>()), Times.Exactly(totalLanguages * totalRepos));
        }

        [Fact]
        public async Task RefreshAsync_TrackedLanguagesIsEmpty_ThrowException()
        {
            // Arrange
            var configurationWithoutLanguages = ConfigurationFactory.GenerateWithoutTrackedLanguages();

            var repoRepository = new Mock<IRepoRepository>();
            repoRepository.Setup(s => s.ClearAsync(It.IsAny<CancellationToken>()));

            var gitHubApiService = new Mock<IGitHubApiService>();

            var repoService = new RepoService(configurationWithoutLanguages, _loggerMock.Object, gitHubApiService.Object, repoRepository.Object, _mapper);


            //Act
            var exception = await Record.ExceptionAsync(() => repoService.RefreshAllAsync(new CancellationToken()));


            //Assert
            Assert.NotNull(exception);
            repoRepository.Verify(s => s.AddAsync(It.IsAny<Repo>(), It.IsAny<CancellationToken>()), Times.Never);
            gitHubApiService.Verify(s => s.FindTopRatedByLangAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RefreshAsync_WhenLanguageRepositoriesNotFound_FindTopRatedByLangAsyncNotCalled()
        {
            // Arrange
            var configurationWithFiveLanguages = ConfigurationFactory.GenerateValid();

            var repoRepository = new Mock<IRepoRepository>();
            repoRepository.Setup(s => s.ClearAsync(It.IsAny<CancellationToken>()));

            var gitHubApiService = new Mock<IGitHubApiService>();
            gitHubApiService.Setup(s => s.FindTopRatedByLangAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GithubRepoSearchModel()
                {
                    IncompleteResults = false,
                    TotalCount = 0,
                    Items = new List<GithubRepoModel>()
                });

            var repoService = new RepoService(configurationWithFiveLanguages, _loggerMock.Object, gitHubApiService.Object, repoRepository.Object, _mapper);


            //Act
            var exception = await Record.ExceptionAsync(() => repoService.RefreshAllAsync(new CancellationToken()));

            //Assert
            Assert.Null(exception);
            repoRepository.Verify(s => s.AddAsync(It.IsAny<Repo>(), It.IsAny<CancellationToken>()), Times.Never);
            gitHubApiService.Verify(s => s.FindTopRatedByLangAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(5));
        }
    }
}
