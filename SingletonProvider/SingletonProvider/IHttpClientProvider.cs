using System;
using System.Net.Http;

namespace SingletonProvider
{
    public interface IHttpClientProvider
    {
        HttpClient Get();
        HttpClient Get(string name);
        void Add(string name, HttpClient client);
        void Add(string name, Func<HttpClient> func);
        void Add(string name, Action<HttpClient> action);
    }
}