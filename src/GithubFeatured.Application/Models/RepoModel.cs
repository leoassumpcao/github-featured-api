namespace GithubFeatured.Application.Models
{
    public class RepoModel
    {
        public Guid Id { get; set; }
        public long GitHubId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool Private { get; set; }
        public string OwnerUser { get; set; }
        public string HtmlUrl { get; set; }
        public string Description { get; set; }
        public bool Fork { get; set; }
        public string Url { get; set; }
        public string Homepage { get; set; }
        public long Size { get; set; }
        public string Language { get; set; }
        public string Visibility { get; set; }
        public uint Score { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset PushedAt { get; set; }

        public long StargazersCount { get; set; }
        public long WatchersCount { get; set; }
        public long ForksCount { get; set; }

        public bool HasIssues { get; set; }
        public uint OpenIssuesCount { get; set; }
        public bool HasProjects { get; set; }
        public bool HasDownloads { get; set; }
        public bool HasWiki { get; set; }
        public bool HasPages { get; set; }
        public bool HasDiscussions { get; set; }

        public bool Archived { get; set; }
        public bool Disabled { get; set; }
    }
}
