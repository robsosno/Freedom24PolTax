using System.Text.Json;
using CommonUtils.Dto;

namespace CommonUtils;

/// <summary>
/// Provides methods to interact with the NBP API to retrieve currency rates.
/// </summary>
public class NBPApi : INBPApi
{
    private readonly Dictionary<string, Tuple<DateOnly, NBPRate>> cache = [];

    /// <summary>
    /// Gets the exchange rate for a specified currency and date.
    /// </summary>
    /// <param name="currency">The currency code (e.g., "USD").</param>
    /// <param name="valueDate">The date for which the rate is requested.</param>
    /// <returns>The exchange rate for the specified currency and date.</returns>
    /// <exception cref="ArgumentException">Thrown when the currency code is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is an error retrieving or deserializing the rate.</exception>
    public NBPRate GetRate(string currency, DateOnly valueDate)
    {
        NBPRate rate;
        if (currency == "PLN")
        {
            rate = new("1", valueDate, currency, 1);
            return rate;
        }

        if (currency == null)
        {
            throw new ArgumentException("brak podanego kodu waluty");
        }

        if (cache.TryGetValue(currency, out Tuple<DateOnly, NBPRate>? pair))
        {
            if (pair.Item1 == valueDate)
            {
                return pair.Item2;
            }
            else
            {
                cache.Remove(currency);
            }
        }

        using var client = RestFactory.HttpClientFactory.CreateClient("NBP");
        HttpResponseMessage httpResponseMessage;
        var date2 = valueDate;
        int maxDays = 10;
        while (true)
        {
            maxDays--;
            date2 = date2.AddDays(-1);
            string action2 = $"{currency}/{date2.ToString("yyyy-MM-dd")}";
            httpResponseMessage = client.GetAsync(action2).Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                break;
            }

            if (httpResponseMessage.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                using var contentStreamErr = httpResponseMessage.Content.ReadAsStreamAsync().Result;
                using var sr = new StreamReader(contentStreamErr);
                var errMsg = sr.ReadToEnd();
                throw new InvalidOperationException(errMsg);
            }

            if (maxDays < 0)
            {
                throw new InvalidOperationException("Brak tabel kursowych w NBP");
            }
        }

        using var contentStream = httpResponseMessage.Content.ReadAsStreamAsync().Result;
        if (JsonSerializer.Deserialize<NbpResponseDto>(contentStream) is NbpResponseDto nbpResponse)
        {
            rate = new("1", valueDate, currency, nbpResponse.RateList[0].Rate);
            return rate;
        }

        throw new InvalidOperationException("deserializacja dała null");
    }
}