using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SingletonProvider
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(Uri requestUri);
    }
}