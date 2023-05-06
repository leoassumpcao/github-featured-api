namespace GithubFeatured.Infra.Services.GitHub.Models
{
    public class GithubRepoOwnerModel
    {
        public long Id { get; set; }
        public string NodeId { get; set; }
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string GravatarId { get; set; }
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public string Type { get; set; }
        public bool SiteAdmin { get; set; }
    }
}
