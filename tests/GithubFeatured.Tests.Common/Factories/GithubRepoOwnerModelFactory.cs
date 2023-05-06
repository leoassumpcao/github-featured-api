using Bogus;
using GithubFeatured.Infra.Services.GitHub.Models;

namespace GithubFeatured.Tests.Common.Factories
{
    public static class GithubRepoOwnerModelFactory
    {
        public static GithubRepoOwnerModel GenerateRandomValid()
        {
            return new GithubRepoOwnerModel()
            {
                Id = new Faker().Random.Long(),
                NodeId = new Faker().Random.String2(10),
                Login = new Faker().Internet.UserNameUnicode()
            };
        }
    }
}
