using System;

namespace CommonUtils.Dto;

public class NBPRate
{
    public static NBPRate None { get; set; } = new();
    public string TableName { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Rate { get; set; }
}