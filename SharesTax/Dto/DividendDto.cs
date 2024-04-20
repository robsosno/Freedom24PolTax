using System;

namespace SharesTax.Dto;

public record DividendDto(
    int Id,
    string Type,
    DateOnly PostingDate,
    string Asset,
    decimal Amount,
    decimal UnitValue,
    string UnitCurrency,
    string Ticker,
    string Isin,
    DateOnly FixationDate,
    int Quantity,
    decimal TaxAmount,
    string TaxCurrency,
    string Comment);