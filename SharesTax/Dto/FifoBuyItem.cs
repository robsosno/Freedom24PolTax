using System;

namespace SharesTax.Dto;

public class FifoBuyItem
{
    public int Id { get; init; }
    public DateOnly ValueDate { get; init; }
    public TimeOnly ValueTime { get; init; }
    public DateOnly PostDate { get; init; }
    public required string Symbol { get; init; }
    public required string Isin { get; init; }
    public int InitialQuantity { get; set; }
    public int Quantity { get; set; }
    public decimal InitialAmount { get; init; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
    public decimal InitialFee { get; init; }
    public required string FeeCurrency { get; set; }
    public decimal Fee { get; set; }
    public IList<FifoSellItem> SellItems { get; init; } = [];
}