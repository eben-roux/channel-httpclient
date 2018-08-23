using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SingletonProvider
{
    public class DelegatingHttpClient : IHttpClient
    {
        private readonly HttpClient _client;

        public DelegatingHttpClient(HttpClient client)
        {
            _client = client;
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return _client.GetAsync(requestUri);
        }
    }
}