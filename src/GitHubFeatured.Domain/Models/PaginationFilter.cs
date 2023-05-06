using System.ComponentModel.DataAnnotations;

namespace GitHubFeatured.Domain.Models
{
    public abstract class PaginationFilter
    {
        private const int MAX_PAGE_SIZE = 100;

        public PaginationFilter()
        { }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : pageSize;
        }

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, MAX_PAGE_SIZE)]
        public int PageSize { get; set; } = 10;
    }
}
