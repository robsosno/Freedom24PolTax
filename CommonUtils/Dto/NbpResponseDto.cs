using System;
using System.Text.Json.Serialization;

namespace CommonUtils.Dto;

/// <summary>
/// Represents the response from the NBP API.
/// </summary>
/// <param name="TableType">The type of the table.</param>
/// <param name="CurrencyName">The name of the currency.</param>
/// <param name="CurrencyCode">The code of the currency.</param>
/// <param name="RateList">The list of rates.</param>
public record NbpResponseDto(
    [property: JsonPropertyName("table")]
    string TableType,
    [property: JsonPropertyName("currency")]
    string CurrencyName,
    [property: JsonPropertyName("code")]
    string CurrencyCode,
    [property: JsonPropertyName("rates")]
    IList<NbpRateResponseDto> RateList);