using System.Text;
using System.Web;

namespace GithubFeatured.Infra.Utils
{
    public static class RequestUriUtil
    {
        public static string GetUriWithQueryString(string requestUri,
            Dictionary<string, string> queryStringParams)
        {
            bool startingQuestionMarkAdded = false;
            var sb = new StringBuilder();
            sb.Append(requestUri);
            foreach (var parameter in queryStringParams)
            {
                if (parameter.Value == null)
                {
                    continue;
                }

                sb.Append(startingQuestionMarkAdded ? '&' : '?');
                sb.Append(parameter.Key);
                sb.Append('=');
                sb.Append(HttpUtility.UrlEncode(parameter.Value));
                startingQuestionMarkAdded = true;
            }
            return sb.ToString();
        }
    }
}
