using System;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonUtils;

/// <summary>
/// Singleton factory class for creating and managing HTTP clients.
/// </summary>
public sealed class RestFactory
{
    private static readonly RestFactory instance = new();
    private readonly IHttpClientFactory httpClientFactory;

    /// <summary>
    /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
    /// </summary>
    static RestFactory()
    {
    }

    /// <summary>
    /// Private constructor to initialize the HTTP client factory with the base URL from configuration.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when baseNbpUrl is not found in configuration.</exception>
    private RestFactory()
    {
        // http://api.nbp.pl/api/exchangerates/rates/a/usd/2016-04-04/?format=json
        // baseUrl = http://api.nbp.pl/api/exchangerates/rates/a
        if (ConfigurationManager.AppSettings["baseNbpUrl"] is string baseUrl)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddHttpClient("NBP", c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                c.DefaultRequestHeaders.Add("Accept", "*/*");
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        }
        else
        {
            throw new InvalidOperationException("Brak baseNbpUrl w konfiguracji");
        }
    }

    /// <summary>
    /// Gets the singleton instance of the HTTP client factory.
    /// </summary>
    public static IHttpClientFactory HttpClientFactory
    {
        get
        {
            return instance.httpClientFactory;
        }
    }
}