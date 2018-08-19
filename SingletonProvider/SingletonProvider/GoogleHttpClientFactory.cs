using System.Net.Http;

namespace SingletonProvider
{
    public class GoogleHttpClientFactory : IHttpClientFactory
    {
        public string Name => "Google";

        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}