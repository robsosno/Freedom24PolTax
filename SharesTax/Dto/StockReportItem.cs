using System;

namespace SharesTax.Dto;

public class StockReportItem
{
    public int GroupId { get; set; }
    public int Id { get; set; }
    public required string Symbol { get; set; }
    public required string Isin { get; set; }
    public int Quantity { get; set; }
    public DateOnly OpenValueDate { get; set; }
    public TimeOnly OpenValueTime { get; set; }
    public DateOnly OpenPostDate { get; set; }
    public decimal BuyAmount { get; set; }
    public decimal BuyFee { get; set; }
    public DateOnly CloseValueDate { get; set; }
    public TimeOnly CloseValueTime { get; set; }
    public DateOnly ClosePostDate { get; set; }
    public decimal SellAmount { get; set; }
    public decimal SellFee { get; set; }
}