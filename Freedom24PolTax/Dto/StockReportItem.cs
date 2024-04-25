using System;

namespace Freedom24PolTax.Dto;

public record StockReportItem(
    int GroupId,
    int Id,
    string Symbol,
    string Isin,
    int Quantity,
    DateOnly OpenValueDate,
    TimeOnly OpenValueTime,
    DateOnly OpenPostDate,
    string BuyCurrency,
    decimal BuyAmount,
    string BuyFeeCurrency,
    decimal BuyFee,
    DateOnly CloseValueDate,
    TimeOnly CloseValueTime,
    DateOnly ClosePostDate,
    string SellCurrency,
    decimal SellAmount,
    string SellFeeCurrency,
    decimal SellFee);