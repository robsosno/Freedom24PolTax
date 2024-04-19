using System;

namespace SharesTax.Dto;

public class DividendDto
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public DateOnly PostingDate { get; set; }
    public required string Asset { get; set; }
    public decimal Amount { get; set; }
    public decimal UnitValue { get; set; }
    public required string UnitCurrency { get; set; }
    public required string Ticker { get; set; }
    public required string Isin { get; set; }
    public DateOnly FixationDate { get; set; }
    public int Quantity { get; set; }
    public decimal TaxAmount { get; set; }
    public required string TaxCurrency { get; set; }
    public required string Comment { get; set; }
}