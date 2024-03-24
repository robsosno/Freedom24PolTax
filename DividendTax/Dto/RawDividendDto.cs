using System;

namespace DividendTax.Dto;

public class RawDividendDto
{
    public string Type { get; set; }
    public DateOnly PostingDate { get; set; }
    public string Asset { get; set; }
    public decimal Amount { get; set; }
    public decimal UnitValue { get; set; }
    public string UnitCurrency { get; set; }
    public string Ticker { get; set; }
    public string Isin { get; set; }
    public DateOnly FixationDate { get; set; }
    public int Quantity { get; set; }
    public string TaxDeductedSource { get; set; }
    public string BrokerTax { get; set; }
    public string Comment { get; set; }
}