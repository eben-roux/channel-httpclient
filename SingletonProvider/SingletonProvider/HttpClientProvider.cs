using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SingletonProvider
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly object Lock = new object();
        private readonly Dictionary<string, HttpClient> _clients = new Dictionary<string, HttpClient>();
        private readonly Dictionary<string, IHttpClientFactory> _factories = new Dictionary<string, IHttpClientFactory>();

        public HttpClientProvider()
        {
            _clients.Add("__default", new HttpClient());
        }

        public HttpClientProvider(IEnumerable<IHttpClientFactory> factories) : this()
        {
            if (factories != null && factories.Any())
            {
                RegisterFactories(factories);
            }
        }

        private void RegisterFactories(IEnumerable<IHttpClientFactory> factories)
        {
            foreach (var factory in factories)
            {
                if (_factories.ContainsKey(factory.Name))
                {
                    _factories.Remove(factory.Name);
                }

                _factories.Add(factory.Name, factory);
            }
        }

        public HttpClient Get()
        {
            return Get("__default");
        }

        public HttpClient Get(string name)
        {
            lock (Lock)
            {
                if (!_clients.ContainsKey(name))
                {
                    if (!_factories.ContainsKey(name))
                    {
                        throw new InvalidOperationException($"Could not find a HttpClient instance named '{name}'.");
                    }

                    _clients.Add(name, _factories[name].Create());
                }

                return _clients[name];
            }
        }

        public void Add(string name, HttpClient client)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Argument 'name' is required.");
            }

            if (client == null)
            {
                throw new ArgumentException("Argument 'client' may not be null");
            }

            lock (Lock)
            {
                if (_clients.ContainsKey(name))
                {
                    throw new InvalidOperationException($"There is already a HttpClient instance named '{name}'.");
                }

                _clients.Add(name, client);
            }
        }
    }
}