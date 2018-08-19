using System;
using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;

namespace SingletonProvider.Tests
{
    [TestFixture]
    public class HttpClientProviderFixture
    {
        [Test]
        public void Should_be_able_to_get_a_new_instance()
        {
            var provider = new HttpClientProvider();

            var client = provider.Get();

            Assert.That(client, Is.Not.Null);
        }

        [Test]
        public void Should_get_the_same_instance()
        {
            var provider = new HttpClientProvider();

            var client1 = provider.Get();
            var client2 = provider.Get();

            Assert.That(client1, Is.SameAs(client2));
        }

        [Test]
        public void Should_throw_an_exception_for_missing_name()
        {
            Assert.That(() => new HttpClientProvider().Get("missing"), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Should_get_the_same_named_instance()
        {
            var provider = new HttpClientProvider();

            provider.Add("my-name", new HttpClient());

            var client1 = provider.Get("my-name");
            var client2 = provider.Get("my-name");

            Assert.That(client1, Is.SameAs(client2));
        }

        [Test]
        public void Should_be_able_to_use_factories()
        {
            var factories = new List<IHttpClientFactory>
            {
                new GoogleHttpClientFactory(),
                new GitHubHttpClientFactory()
            };

            var provider = new HttpClientProvider(factories);

            var github = provider.Get("GitHub");
            var google = provider.Get("Google");

            Assert.That(github, Is.Not.Null);
            Assert.That(google, Is.Not.Null);

            Assert.That(github, Is.Not.SameAs(google));

            var github2 = provider.Get("GitHub");
            var google2 = provider.Get("Google");

            Assert.That(github, Is.SameAs(github2));
            Assert.That(google, Is.SameAs(google2));
        }
    }
}