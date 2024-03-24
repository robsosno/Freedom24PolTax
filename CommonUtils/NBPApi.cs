using System.Text.Json;
using CommonUtils.Dto;

namespace CommonUtils;

public class NBPApi : INBPApi
{
    private readonly Dictionary<string, Tuple<DateOnly, NBPRate>> cache = [];

    public NBPRate GetRate(string currency, DateOnly valueDate)
    {
        NBPRate rate;
        if (currency == "PLN")
        {
            rate = new();
            rate.TableName = "1";
            rate.Date = valueDate;
            rate.Currency = currency;
            rate.Rate = 1;
            return rate;
        }

        if (currency == null)
        {
            throw new ArgumentException("brak podanego kodu waluty");
        }

        if (cache.ContainsKey(currency))
        {
            var pair = cache[currency];
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
            string action2 = currency + "/" + date2.ToString("yyyy-MM-dd");
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

            if (maxDays<0)
            {
                throw new InvalidOperationException("Brak tabel kursowych w NBP");
            }
        }

        using var contentStream = httpResponseMessage.Content.ReadAsStreamAsync().Result;
        if (JsonSerializer.Deserialize<NbpResponseDto>(contentStream) is NbpResponseDto nbpResponse)
        {
            rate = new();
            rate.TableName = "1";
            rate.Date = valueDate;
            rate.Currency = currency;
            rate.Rate = nbpResponse.RateList[0].Rate;
            return rate;
        }

        throw new InvalidOperationException("deserializacja dała null");            
    }
}