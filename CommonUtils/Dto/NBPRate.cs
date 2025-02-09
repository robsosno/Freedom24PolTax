using System;

namespace CommonUtils.Dto;

/// <summary>
/// Represents an NBP (National Bank of Poland) rate.
/// </summary>
/// <param name="TableName">The name of the table containing the rate.</param>
/// <param name="Date">The date of the rate.</param>
/// <param name="Currency">The currency of the rate.</param>
/// <param name="Rate">The value of the rate.</param>
public record NBPRate(string TableName, DateOnly Date, string Currency, decimal Rate);