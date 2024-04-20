using System;
using System.Text.Json.Serialization;

namespace CommonUtils.Dto;

public record NbpResponseDto(
    [property: JsonPropertyName("table")]
    string TableType,
    [property: JsonPropertyName("currency")]
    string CurrencyName,
    [property: JsonPropertyName("code")]
    string CurrencyCode,
    [property: JsonPropertyName("rates")]
    IList<NbpRateResponseDto> RateList);