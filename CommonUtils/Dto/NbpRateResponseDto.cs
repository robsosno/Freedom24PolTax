using System.Text.Json.Serialization;

namespace CommonUtils.Dto;

/// <summary>
/// Represents the response DTO for NBP rate.
/// </summary>
/// <param name="TableNumber">The table number of the rate.</param>
/// <param name="EffectiveDate">The effective date of the rate.</param>
/// <param name="Rate">The mid rate.</param>
public record NbpRateResponseDto(
    [property: JsonPropertyName("no")]
    string TableNumber,
    [property: JsonPropertyName("effectiveDate")]
    string EffectiveDate,
    [property: JsonPropertyName("mid")]
    decimal Rate);