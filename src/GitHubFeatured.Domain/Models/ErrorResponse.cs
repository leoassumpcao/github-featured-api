namespace GitHubFeatured.Domain.Models
{
    public class ErrorResponse : Response<object>
    {
        public ErrorResponse() : base(null)
        {
            Succeeded = false;
            Errors = null;
        }

        public ErrorResponse(string errorMessage) : base(null)
        {
            Succeeded = false;
            Errors = new List<string>() { errorMessage };
        }

        public ErrorResponse(IEnumerable<string> errors) : base(null)
        {
            Succeeded = false;
            Errors = errors;
        }
    }
}
