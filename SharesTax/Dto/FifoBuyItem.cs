using System;

namespace SharesTax.Dto;

public class FifoBuyItem
{
    public int Id { get; set; }
    public DateOnly ValueDate { get; set; }
    public TimeOnly ValueTime { get; set; }
    public DateOnly PostDate { get; set; }
    public required string Symbol { get; set; }
    public required string Isin { get; set; }
    public int InitialQuantity { get; set; }
    public int Quantity { get; set; }
    public decimal InitialBuyAmount { get; set; }
    public decimal BuyAmount { get; set; }
    public decimal InitialBuyFee { get; set; }
    public decimal BuyFee { get; set; }
    public IList<FifoSellItem> SellItems { get; set; } = [];
}