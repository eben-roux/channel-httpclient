using System.Net.Http;

namespace SingletonProvider
{
    public interface IHttpClientFactory
    {
        string Name { get; }
        HttpClient Create();
    }
}