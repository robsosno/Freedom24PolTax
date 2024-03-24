using System;

namespace SharesTax.Dto;

public class FifoSellItem
{
    public int GroupId { get; set; }
    public int Id { get; set; }
    public DateOnly CloseDate { get; set; }
    public int Quantity { get; set; }
    public decimal BuyAmount { get; set; }
    public decimal SellAmount { get; set; }
    public decimal BuyFee { get; set; }
    public decimal SellFee { get; set; }
}