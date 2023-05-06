namespace GitHubFeatured.Domain.Models
{
    public class PagedResponse<T> : Response<IEnumerable<T>>
    {
        public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize) : base(data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
