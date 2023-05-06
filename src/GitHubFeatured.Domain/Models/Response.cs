using System.Text.Json.Serialization;

namespace GitHubFeatured.Domain.Models
{
    public class Response<T>
    {
        public Response()
        {
            Succeeded = true;
            Errors = null;
        }

        public Response(T data)
        {
            Succeeded = true;
            Errors = null;
            Data = data;
        }

        public T Data { get; set; }
        public bool Succeeded { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> Errors { get; set; }
    }
}
