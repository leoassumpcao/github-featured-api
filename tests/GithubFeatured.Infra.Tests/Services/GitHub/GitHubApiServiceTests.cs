using GithubFeatured.Infra.Interfaces;
using GithubFeatured.Infra.Services.GitHub;
using GithubFeatured.Infra.Services.GitHub.Exceptions;
using GithubFeatured.Infra.Services.GitHub.Models;
using GithubFeatured.Tests.Common.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace GithubFeatured.Infra.Tests.Services.RepoServiceTests
{
    public class GitHubApiServiceTests
    {
        private readonly Mock<ILogger<GitHubApiService>> _loggerMock;
        private readonly IConfiguration _configuration;

        public GitHubApiServiceTests()
        {
            _loggerMock = new Mock<ILogger<GitHubApiService>>();

            _configuration = ConfigurationFactory.GenerateValid();
        }

        private HttpClient GeneratedMockedClient(HttpResponseMessage httpResponseMessage)
        {
            var mockedHandler = new Mock<HttpMessageHandler>();
            mockedHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(httpResponseMessage);

            return new HttpClient(mockedHandler.Object)
            {
                BaseAddress = new Uri("https://www.google.com"),
            };
        }

        [Fact]
        public async Task FindTopRatedByLangAsync_ReturnItems()
        {
            //Arrange
            var expectedContent = new GithubRepoSearchModel()
            {
                IncompleteResults = true,
                TotalCount = 500,
                Items = GithubRepoModelFactory.GenerateMultipleRandomValid(5)
            };
            var expectedResponse = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedContent))
            };

            var httpClient = GeneratedMockedClient(expectedResponse);
            var service = new GitHubApiService(httpClient, _configuration, _loggerMock.Object);


            //Act
            var result = await service.FindTopRatedByLangAsync("PHP", 10, 1, new CancellationToken());


            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
        }

        [Fact]
        public async Task FindTopRatedByLangAsync_ReturnEmptyItems()
        {
            //Arrange
            var expectedContent = new GithubRepoSearchModel()
            {
                IncompleteResults = true,
                TotalCount = 0,
                Items = new List<GithubRepoModel>()
            };
            var expectedResponse = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedContent))
            };

            var httpClient = GeneratedMockedClient(expectedResponse);
            var service = new GitHubApiService(httpClient, _configuration, _loggerMock.Object);


            //Act
            var result = await service.FindTopRatedByLangAsync("PHP", 10, 1, new CancellationToken());


            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task FindTopRatedByLangAsync_WhenInvalidStatusCode_ThrowException()
        {
            //Arrange
            var expectedResponse = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Content = new StringContent("")
            };

            var httpClient = GeneratedMockedClient(expectedResponse);
            IGitHubApiService service = new GitHubApiService(httpClient, _configuration, _loggerMock.Object);


            //Act
            var exception = await Record.ExceptionAsync(() => service.FindTopRatedByLangAsync("PHP"));


            //Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void FindTopRatedByLangAsync_WhenGitHubApiKeyIsInvalid_ThrowException()
        {
            //Arrange
            var expectedResponse = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Content = new StringContent("")
            };


            //Act
            Exception exception = Assert.Throws<GithubApiException>(() => new GitHubApiService(ConfigurationFactory.GenerateWithoutGitHubApiKey(), _loggerMock.Object));


            //Assert
            Assert.NotNull(exception);
        }
    }
}
