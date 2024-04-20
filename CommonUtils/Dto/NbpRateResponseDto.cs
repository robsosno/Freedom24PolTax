using System.Text.Json.Serialization;

namespace CommonUtils.Dto;

public record NbpRateResponseDto(
    [property: JsonPropertyName("no")]
    string TableNumber,
    [property: JsonPropertyName("effectiveDate")]
    string EffectiveDate,
    [property: JsonPropertyName("mid")]
    decimal Rate);