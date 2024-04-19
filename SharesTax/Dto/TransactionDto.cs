using System;

namespace SharesTax.Dto;

public enum OrderType
{
    None,
    Buy,
    Sell,
    Split
}

public class TransactionDto
{
    public int Id { get; set; }
    public required string TradeNo { get; init; }
    public DateOnly TransactionDate { get; init; }
    public TimeOnly TransactionTime { get; init; }
    public DateOnly PostingDate { get; init; }
    public required string Ticker { get; init; }
    public required string Isin { get; init; }
    public OrderType OrderType { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public required string AmountCurrency { get; init; }
    public decimal Amount { get; init; }
    public decimal Profit { get; init; }
    public required string CommissionCurrency { get; init; }
    public decimal Commission { get; init; }
}