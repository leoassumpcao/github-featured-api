using AutoMapper;
using GithubFeatured.Application.Mapping;
using GithubFeatured.Application.Services;
using GithubFeatured.Infra.Interfaces;
using GithubFeatured.Tests.Common.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace GithubFeatured.Application.Tests.Services.RepoServiceTests
{
    public class RepoServiceGetByIdAsyncTests
    {
        private readonly Mock<ILogger<RepoService>> _loggerMock;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly Mock<IRepoRepository> _repoRepositoryMock;

        public RepoServiceGetByIdAsyncTests()
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
        public async Task GetByIdAsync_Sucess()
        {
            // Arrange
            var repoRepository = new Mock<IRepoRepository>();
            repoRepository.Setup(s => s.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(RepoFactory.GenerateRandomValid());

            var gitHubApiService = new Mock<IGitHubApiService>();

            var repoService = new RepoService(_configuration, _loggerMock.Object, gitHubApiService.Object, repoRepository.Object, _mapper);


            //Act
            var result = await repoService.GetByIdAsync(Guid.NewGuid(), new CancellationToken());


            //Assert
            Assert.NotNull(result);
            repoRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_EmptyResult()
        {
            // Arrange
            var repoRepository = new Mock<IRepoRepository>();
            repoRepository.Setup(s => s.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>())
            );

            var gitHubApiService = new Mock<IGitHubApiService>();

            var repoService = new RepoService(_configuration, _loggerMock.Object, gitHubApiService.Object, repoRepository.Object, _mapper);


            //Act
            var result = await repoService.GetByIdAsync(Guid.NewGuid(), new CancellationToken());


            //Assert
            Assert.Null(result);
            repoRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
