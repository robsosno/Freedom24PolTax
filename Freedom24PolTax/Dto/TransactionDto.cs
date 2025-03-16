using System;

namespace Freedom24PolTax.Dto;

/// <summary>
/// Represents the type of order in a transaction.
/// </summary>
public enum OrderType
{
    /// <summary>
    /// No order type specified.
    /// </summary>
    None,
    /// <summary>
    /// Buy order type.
    /// </summary>
    Buy,
    /// <summary>
    /// Sell order type.
    /// </summary>
    Sell,
    /// <summary>
    /// Split order type.
    /// </summary>
    Split
}

/// <summary>
/// Data transfer object for a transaction.
/// </summary>
/// <param name="Id">The unique identifier of the transaction. Example: 12</param>
/// <param name="TradeNo">The trade number associated with the transaction. Example: "321910003"</param>
/// <param name="TransactionDate">The date when the transaction occurred. Example: 2023-05-30</param>
/// <param name="TransactionTime">The time when the transaction occurred. Example: 20:43:33</param>
/// <param name="PostingDate">The date when the transaction was posted. Example: 2023-06-01</param>
/// <param name="Ticker">The ticker symbol of the security involved in the transaction. Example: "URNM.US"</param>
/// <param name="Isin">The ISIN code of the security involved in the transaction. Example: "US85208P3038"</param>
/// <param name="OrderType">The type of order for the transaction. Example: buy</param>
/// <param name="Quantity">The quantity of securities involved in the transaction. Example: 30</param>
/// <param name="Price">The price per unit of the security. Example: 29.0516</param>
/// <param name="AmountCurrency">The currency of the transaction amount. Example: "USD"</param>
/// <param name="Amount">The total amount of the transaction. Example: 871.55</param>
/// <param name="Profit">The profit from the transaction. Example: 0.00000000</param>
/// <param name="CommissionCurrency">The currency of the commission. Example: "USD"</param>
/// <param name="Commission">The commission for the transaction. Example: 0</param>
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