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
/// <param name="Id">The unique identifier of the transaction.</param>
/// <param name="TradeNo">The trade number associated with the transaction.</param>
/// <param name="TransactionDate">The date when the transaction occurred.</param>
/// <param name="TransactionTime">The time when the transaction occurred.</param>
/// <param name="PostingDate">The date when the transaction was posted.</param>
/// <param name="Ticker">The ticker symbol of the security involved in the transaction.</param>
/// <param name="Isin">The ISIN code of the security involved in the transaction.</param>
/// <param name="OrderType">The type of order for the transaction.</param>
/// <param name="Quantity">The quantity of securities involved in the transaction.</param>
/// <param name="Price">The price per unit of the security.</param>
/// <param name="AmountCurrency">The currency of the transaction amount.</param>
/// <param name="Amount">The total amount of the transaction.</param>
/// <param name="Profit">The profit from the transaction.</param>
/// <param name="CommissionCurrency">The currency of the commission.</param>
/// <param name="Commission">The commission for the transaction.</param>
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