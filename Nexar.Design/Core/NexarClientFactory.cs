using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Net.Http.Headers;

namespace Nexar.Client
{
    /// <summary>
    /// Getting configured Nexar clients.
    /// </summary>
    public static class NexarClientFactory
    {
        static readonly ConcurrentDictionary<string, NexarClient> _clients = new ConcurrentDictionary<string, NexarClient>();

        /// <summary>
        /// Nexar access token, must be assigned.
        /// </summary>
        public static string AccessToken { get; set; }

        /// <summary>
        /// Gets the specified client.
        /// </summary>
        public static NexarClient GetClient(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentNullException(nameof(endpoint));

            if (string.IsNullOrEmpty(AccessToken))
                throw new ArgumentNullException(nameof(AccessToken));

            var endpointUri = new Uri(endpoint);
            return _clients.GetOrAdd(endpointUri.AbsoluteUri, _ => CreateClient(endpointUri));
        }

        private static NexarClient CreateClient(Uri endpoint)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddNexarClient()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = endpoint;
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                });

            var services = serviceCollection.BuildServiceProvider();
            return services.GetRequiredService<NexarClient>();
        }
    }
}
