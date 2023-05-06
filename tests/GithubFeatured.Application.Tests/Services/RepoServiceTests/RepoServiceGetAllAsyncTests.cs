using AutoMapper;
using GithubFeatured.Application.Mapping;
using GithubFeatured.Application.Services;
using GithubFeatured.Infra.Interfaces;
using GithubFeatured.Tests.Common.Factories;
using GitHubFeatured.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace GithubFeatured.Application.Tests.Services.RepoServiceTests
{
    public class RepoServiceGetAllAsyncTests
    {
        private readonly Mock<ILogger<RepoService>> _loggerMock;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly Mock<IRepoRepository> _repoRepositoryMock;

        public RepoServiceGetAllAsyncTests()
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
        public async Task GetAllAsync_Sucess()
        {
            // Arrange
            var repoRepository = new Mock<IRepoRepository>();
            repoRepository.Setup(s => s.GetAllPaginatedAsync(
                It.IsAny<IQueryable<Repo>>(),
                It.IsAny<Models.GetAllRepoFilterModel>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(new List<Repo>() { RepoFactory.GenerateRandomValid() });

            var gitHubApiService = new Mock<IGitHubApiService>();

            var repoService = new RepoService(_configuration, _loggerMock.Object, gitHubApiService.Object, repoRepository.Object, _mapper);


            //Act
            var result = await repoService.GetAllAsync(new Models.GetAllRepoFilterModel(), new CancellationToken());


            //Assert
            Assert.NotEmpty(result.Data);
            repoRepository.Verify(repo => repo.GetQueryable(), Times.Once);
            repoRepository.Verify(repo => repo.GetAllPaginatedAsync(It.IsAny<IQueryable<Repo>>(), It.IsAny<Models.GetAllRepoFilterModel>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_EmptyResult()
        {
            // Arrange
            var repoRepository = new Mock<IRepoRepository>();
            repoRepository.Setup(s => s.GetAllPaginatedAsync(
                It.IsAny<IQueryable<Repo>>(),
                It.IsAny<Models.GetAllRepoFilterModel>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(new List<Repo>());

            var gitHubApiService = new Mock<IGitHubApiService>();

            var repoService = new RepoService(_configuration, _loggerMock.Object, gitHubApiService.Object, repoRepository.Object, _mapper);


            //Act
            var result = await repoService.GetAllAsync(new Models.GetAllRepoFilterModel(), new CancellationToken());


            //Assert
            Assert.Empty(result.Data);
            repoRepository.Verify(repo => repo.GetQueryable(), Times.Once);
            repoRepository.Verify(repo => repo.GetAllPaginatedAsync(It.IsAny<IQueryable<Repo>>(), It.IsAny<Models.GetAllRepoFilterModel>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
