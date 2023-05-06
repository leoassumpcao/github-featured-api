using Bogus;
using GithubFeatured.Infra.Services.GitHub.Models;

namespace GithubFeatured.Tests.Common.Factories
{
    public static class GithubRepoModelFactory
    {
        public static GithubRepoModel GenerateRandomValid()
        {
            return new GithubRepoModel()
            {
                Id = new Faker().Random.Long(),
                NodeId = new Faker().Random.String2(10),
                Name = new Faker().Internet.UserNameUnicode(),
                FullName = new Faker().Internet.UserNameUnicode(),
                Archived = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PushedAt = DateTime.UtcNow,
                Description = new Faker().Lorem.Text(),
                Disabled = false,
                Fork = false,
                ForksCount = 0,
                Language = new Faker().Random.ArrayElement(new string[] { "PHP", "C#", "JavaScript" }),
                Owner = GithubRepoOwnerModelFactory.GenerateRandomValid(),
                Score = 1,
                StargazersCount = 500,
                Private = false,
            };
        }

        public static IEnumerable<GithubRepoModel> GenerateMultipleRandomValid(int count)
        {
            var githubRepoModels = new List<GithubRepoModel>();
            for (int i = 0; i < count; i++)
            {
                githubRepoModels.Add(GenerateRandomValid());
            }

            return githubRepoModels;
        }
    }
}
