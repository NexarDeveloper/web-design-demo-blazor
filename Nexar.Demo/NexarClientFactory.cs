using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Nexar.Client;

/// <summary>
/// Getting configured Nexar clients.
/// </summary>
public static class NexarClientFactory
{
    static readonly ConcurrentDictionary<string, NexarClient> _clients = new();

    /// <summary>
    /// Nexar access token, must be assigned.
    /// </summary>
    public static string? AccessToken { get; set; }

    /// <summary>
    /// Gets the specified client.
    /// </summary>
    public static NexarClient GetClient(string endpoint)
    {
        if (string.IsNullOrEmpty(endpoint))
            throw new ArgumentNullException(nameof(endpoint));

        if (string.IsNullOrEmpty(AccessToken))
            throw new InvalidOperationException(nameof(AccessToken));

        var endpointUri = new Uri(endpoint);
        return _clients.GetOrAdd(endpointUri.AbsoluteUri, _ => CreateClient(endpointUri));
    }

    private static NexarClient CreateClient(Uri endpoint)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddNexarClient()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = endpoint;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");
            });

        var services = serviceCollection.BuildServiceProvider();
        return services.GetRequiredService<NexarClient>();
    }
}
