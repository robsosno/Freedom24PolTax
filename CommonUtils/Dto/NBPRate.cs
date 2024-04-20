using System;

namespace CommonUtils.Dto;

public record NBPRate(string TableName, DateOnly Date, string Currency, decimal Rate);