using System;

namespace SharesTax.Dto;

public enum OrderType
{
    None,
    Buy,
    Sell,
    Split
}

public record TransactionDto(
    int Id,
    string TradeNo,
    DateOnly TransactionDate,
    TimeOnly TransactionTime,
    DateOnly PostingDate,
    string Ticker,
    string Isin,
    OrderType OrderType,
    int Quantity,
    decimal Price,
    string AmountCurrency,
    decimal Amount,
    decimal Profit,
    string CommissionCurrency,
    decimal Commission);