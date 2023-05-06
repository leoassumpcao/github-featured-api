using Bogus;
using GitHubFeatured.Domain.Entities;

namespace GithubFeatured.Tests.Common.Factories
{
    public static class RepoFactory
    {
        public static Repo GenerateRandomValid()
        {
            return new Repo()
            {
                GitHubId = new Faker().Random.Long(),
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
                OwnerUser = new Faker().Internet.UserNameUnicode(),
                Score = 1,
                StargazersCount = 500,
                Private = false,
            };
        }

        public static Repo GenerateRandomValidWithSpecifiedLanguage(string language)
        {
            var result = GenerateRandomValid();
            result.Language = language;

            return result;
        }

        public static IEnumerable<Repo> GenerateMultipleRandomValid(int count)
        {
            var githubRepoModels = new List<Repo>();
            for (int i = 0; i < count; i++)
            {
                githubRepoModels.Add(GenerateRandomValid());
            }

            return githubRepoModels;
        }
    }
}
