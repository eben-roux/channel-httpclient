using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SingletonProvider
{
    public class MockableHttpClient : HttpClient, IHttpClient
    {
        public new Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return base.GetAsync(requestUri);
        }
    }
}