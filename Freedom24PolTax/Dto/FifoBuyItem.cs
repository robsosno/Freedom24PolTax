using System;

namespace Freedom24PolTax.Dto;

/// <summary>
/// Represents an item in a FIFO (First In, First Out) buy transaction.
/// </summary>
public class FifoBuyItem
{
    /// <summary>
    /// Gets the unique identifier of the buy item.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the date when the value is recorded.
    /// </summary>
    public DateOnly ValueDate { get; init; }

    /// <summary>
    /// Gets the time when the value is recorded.
    /// </summary>
    public TimeOnly ValueTime { get; init; }

    /// <summary>
    /// Gets the date when the transaction is posted.
    /// </summary>
    public DateOnly PostDate { get; init; }

    /// <summary>
    /// Gets the symbol of the security.
    /// </summary>
    public required string Symbol { get; init; }

    /// <summary>
    /// Gets the ISIN (International Securities Identification Number) of the security.
    /// </summary>
    public required string Isin { get; init; }

    /// <summary>
    /// Gets or sets the initial quantity of items bought.
    /// </summary>
    public int InitialQuantity { get; set; }

    /// <summary>
    /// Gets or sets the current quantity of items.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets the initial amount spent to buy the items.
    /// </summary>
    public decimal InitialAmount { get; init; }

    /// <summary>
    /// Gets or sets the currency of the transaction.
    /// </summary>
    public required string Currency { get; set; }

    /// <summary>
    /// Gets or sets the current amount of the transaction.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets the initial fee paid during the purchase.
    /// </summary>
    public decimal InitialFee { get; init; }

    /// <summary>
    /// Gets or sets the currency in which the fee is paid.
    /// </summary>
    public required string FeeCurrency { get; set; }

    /// <summary>
    /// Gets or sets the current fee amount for the transaction.
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Gets the list of sell items associated with this buy item.
    /// </summary>
    public IList<FifoSellItem> SellItems { get; init; } = [];
}