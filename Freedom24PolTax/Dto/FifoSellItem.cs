using System;

namespace Freedom24PolTax.Dto;

/// <summary>
/// Represents an item in a FIFO (First In, First Out) sell transaction.
/// </summary>
/// <param name="GroupId">The group identifier.</param>
/// <param name="Id">The unique identifier of the sell item.</param>
/// <param name="ValueDate">The date when the value is recorded.</param>
/// <param name="ValueTime">The time when the value is recorded.</param>
/// <param name="PostDate">The date when the transaction is posted.</param>
/// <param name="Quantity">The quantity of items sold.</param>
/// <param name="BuyAmount">The amount spent to buy the items.</param>
/// <param name="Currency">The currency of the transaction.</param>
/// <param name="Amount">The amount received from the sale.</param>
/// <param name="BuyFee">The fee paid during the purchase.</param>
/// <param name="FeeCurrency">The currency in which the fee is paid.</param>
/// <param name="Fee">The fee amount for the transaction.</param>
public record FifoSellItem(
    int GroupId,
    int Id,
    DateOnly ValueDate,
    TimeOnly ValueTime,
    DateOnly PostDate,
    int Quantity,
    decimal BuyAmount,
    string Currency,
    decimal Amount,
    decimal BuyFee,
    string FeeCurrency,
    decimal Fee);