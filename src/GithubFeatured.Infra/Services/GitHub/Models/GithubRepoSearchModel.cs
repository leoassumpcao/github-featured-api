namespace GithubFeatured.Infra.Services.GitHub.Models
{
    public class GithubRepoSearchModel
    {
        public long TotalCount { get; set; }
        public bool IncompleteResults { get; set; }
        public IEnumerable<GithubRepoModel> Items { get; set; }
    }
}
