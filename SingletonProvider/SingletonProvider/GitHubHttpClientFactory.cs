using System.Net.Http;

namespace SingletonProvider
{
    public class GitHubHttpClientFactory : IHttpClientFactory
    {
        public string Name => "GitHub";

        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}