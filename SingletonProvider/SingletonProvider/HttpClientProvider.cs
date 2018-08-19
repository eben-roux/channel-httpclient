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
        private readonly Dictionary<string, Func<HttpClient>> _callbackCreate = new Dictionary<string, Func<HttpClient>>();
        private readonly Dictionary<string, Action<HttpClient>> _callbackConfigure = new Dictionary<string, Action<HttpClient>>();

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
                    if (_factories.ContainsKey(name))
                    {
                        _clients.Add(name, _factories[name].Create());
                    }

                    if (!_clients.ContainsKey(name) && _callbackCreate.ContainsKey(name))
                    {
                        _clients.Add(name, _callbackCreate[name].Invoke());
                    }

                    if (!_clients.ContainsKey(name) && _callbackConfigure.ContainsKey(name))
                    {
                        var client = new HttpClient();

                        _callbackConfigure[name].Invoke(client);

                        _clients.Add(name, client);
                    }

                    if (!_clients.ContainsKey(name))
                    {
                        throw new InvalidOperationException($"Could not find a HttpClient instance named '{name}'.");
                    }
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

        public void Add(string name, Func<HttpClient> func)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Argument 'name' is required.");
            }

            if (func == null)
            {
                throw new ArgumentException("Argument 'func' may not be null");
            }

            lock (Lock)
            {
                if (_callbackCreate.ContainsKey(name))
                {
                    _callbackCreate.Remove(name);
                }

                _callbackCreate.Add(name, func);
            }
        }

        public void Add(string name, Action<HttpClient> action)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Argument 'name' is required.");
            }

            if (action == null)
            {
                throw new ArgumentException("Argument 'action' may not be null");
            }

            lock (Lock)
            {
                if (_callbackConfigure.ContainsKey(name))
                {
                    _callbackConfigure.Remove(name);
                }

                _callbackConfigure.Add(name, action);
            }
        }
    }
}