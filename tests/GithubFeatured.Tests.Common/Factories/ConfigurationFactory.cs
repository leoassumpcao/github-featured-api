using Microsoft.Extensions.Configuration;

namespace GithubFeatured.Tests.Common.Factories
{
    public static class ConfigurationFactory
    {
        public static IConfiguration GenerateValid()
        {
            var appSettings = new Dictionary<string, string> {
                {"GitHub:ApiKey", Guid.NewGuid().ToString()},
                {"TrackedLanguages:0", "PHP"},
                {"TrackedLanguages:1", "C#"},
                {"TrackedLanguages:2", "JavaScript"},
                {"TrackedLanguages:3", "C++"},
                {"TrackedLanguages:4", "Dart"},
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(appSettings)
                .Build();
        }

        public static IConfiguration GenerateWithoutGitHubApiKey()
        {
            var appSettings = new Dictionary<string, string>
            {
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(appSettings)
                .Build();
        }

        public static IConfiguration GenerateWithoutTrackedLanguages()
        {
            var appSettings = new Dictionary<string, string>
            {

            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(appSettings)
                .Build();
        }
    }
}
