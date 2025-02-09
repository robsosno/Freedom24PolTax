using System;

namespace Freedom24PolTax.Dto;

/// <summary>
/// Represents an item in a stock report.
/// </summary>
/// <param name="GroupId">The group identifier.</param>
/// <param name="Id">The unique identifier of the stock report item.</param>
/// <param name="Symbol">The stock symbol.</param>
/// <param name="Isin">The International Securities Identification Number (ISIN).</param>
/// <param name="Quantity">The quantity of stocks.</param>
/// <param name="OpenValueDate">The date when the stock was opened.</param>
/// <param name="OpenValueTime">The time when the stock was opened.</param>
/// <param name="OpenPostDate">The date when the stock was posted as open.</param>
/// <param name="BuyCurrency">The currency used to buy the stock.</param>
/// <param name="BuyAmount">The amount used to buy the stock.</param>
/// <param name="BuyFeeCurrency">The currency used for the buying fee.</param>
/// <param name="BuyFee">The fee for buying the stock.</param>
/// <param name="CloseValueDate">The date when the stock was closed.</param>
/// <param name="CloseValueTime">The time when the stock was closed.</param>
/// <param name="ClosePostDate">The date when the stock was posted as closed.</param>
/// <param name="SellCurrency">The currency used to sell the stock.</param>
/// <param name="SellAmount">The amount received from selling the stock.</param>
/// <param name="SellFeeCurrency">The currency used for the selling fee.</param>
/// <param name="SellFee">The fee for selling the stock.</param>
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