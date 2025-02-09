using System;
using CommonUtils.Dto;

namespace CommonUtils;

/// <summary>
/// Interface for accessing NBP (National Bank of Poland) API to retrieve exchange rates.
/// </summary>
public interface INBPApi
{
    /// <summary>
    /// Gets the exchange rate for a specified billing currency and date.
    /// </summary>
    /// <param name="billingCurrency">The currency for which the exchange rate is requested.</param>
    /// <param name="valueDate">The date for which the exchange rate is requested.</param>
    /// <returns>An <see cref="NBPRate"/> object containing the exchange rate information.</returns>
    NBPRate GetRate(string billingCurrency, DateOnly valueDate);
}