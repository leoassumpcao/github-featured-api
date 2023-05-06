namespace GithubFeatured.Infra.Services.GitHub.Exceptions
{
    public class GithubApiException : Exception
    {
        public GithubApiException() : base("Error communicating with GitHub API")
        { }

        public GithubApiException(string message) : base(message) { }
    }
}
