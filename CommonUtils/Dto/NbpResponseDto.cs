using System;
using System.Text.Json.Serialization;

namespace CommonUtils.Dto;

public class NbpResponseDto
{
    [JsonPropertyName("table")]
    public string TableType { get; set; }
    [JsonPropertyName("currency")]
    public string CurrencyName { get; set; }
    [JsonPropertyName("code")]
    public string CurrencyCode { get; set; }
    [JsonPropertyName("rates")]
    public IList<NbpRateResponseDto> RateList { get; set; }
}