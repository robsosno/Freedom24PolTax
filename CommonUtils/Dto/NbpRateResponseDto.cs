using System.Text.Json.Serialization;

namespace CommonUtils.Dto;

public class NbpRateResponseDto
{
    [JsonPropertyName("no")]
    public string TableNumber { get; set; }
    [JsonPropertyName("effectiveDate")]
    public string EffectiveDate { get; set; }
    [JsonPropertyName("mid")]
    public decimal Rate { get; set; }
}