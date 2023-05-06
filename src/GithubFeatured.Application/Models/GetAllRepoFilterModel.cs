using GitHubFeatured.Domain.Models;

namespace GithubFeatured.Application.Models
{
    public class GetAllRepoFilterModel : PaginationFilter
    {
        public string Language { get; set; }
    }
}
