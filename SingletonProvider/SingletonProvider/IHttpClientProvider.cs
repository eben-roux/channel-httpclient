using System.Net.Http;

namespace SingletonProvider
{
    public interface IHttpClientProvider
    {
        HttpClient Get();
        HttpClient Get(string name);
        void Add(string name, HttpClient client);
    }
}