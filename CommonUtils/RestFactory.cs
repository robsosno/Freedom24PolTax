using System;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonUtils;

public sealed class RestFactory
{
    private static readonly RestFactory instance = new();
    private readonly IHttpClientFactory httpClientFactory;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static RestFactory()
    {
    }

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

    public static IHttpClientFactory HttpClientFactory
    {
        get
        {
            return instance.httpClientFactory;
        }
    }
}