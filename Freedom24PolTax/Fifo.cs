using System;
using Freedom24PolTax.Dto;

namespace Freedom24PolTax;

/// <summary>
/// Represents a FIFO (First In, First Out) inventory management system.
/// </summary>
public class Fifo
{
    /// <summary>
    /// Gets or sets the list of FIFO buy items.
    /// </summary>
    public IList<FifoBuyItem> FifoItems { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Fifo"/> class.
    /// </summary>
    public Fifo()
    {
        FifoItems = [];
    }

    private int maxId = 0;

    /// <summary>
    /// Adds a new buy item to the FIFO list.
    /// </summary>
    /// <param name="valueDate">The value date of the buy item.</param>
    /// <param name="valueTime">The value time of the buy item.</param>
    /// <param name="postDate">The post date of the buy item.</param>
    /// <param name="symbol">The symbol of the buy item.</param>
    /// <param name="isin">The ISIN of the buy item.</param>
    /// <param name="quantity">The quantity of the buy item.</param>
    /// <param name="price">The price of the buy item.</param>
    /// <param name="feeCurrency">The currency of the fees.</param>
    /// <param name="fees">The fees associated with the buy item.</param>
    /// <param name="currency">The currency of the buy item.</param>
    /// <param name="amount">The total amount of the buy item.</param>
    /// <exception cref="ArgumentException">Thrown when the calculated amount does not match the provided amount.</exception>
    public void Push(
                    DateOnly valueDate,
                    TimeOnly valueTime,
                    DateOnly postDate,
                    string symbol,
                    string isin,
                    int quantity,
                    decimal price,
                    string feeCurrency,
                    decimal fees,
                    string currency,
                    decimal amount)
    {
        if (Math.Abs(Math.Round(price * quantity - amount)) > 0.01M)
        {
            throw new ArgumentException("Cena * ilosc <> amount", nameof(price));
        }

        FifoBuyItem item = new()
        {
            Id = maxId++,
            ValueDate = valueDate,
            ValueTime = valueTime,
            PostDate = postDate,
            Symbol = symbol,
            Isin = isin,
            InitialQuantity = quantity,
            Quantity = quantity,
            InitialAmount = Math.Round(amount, 2, MidpointRounding.AwayFromZero),
            Currency = currency,
            Amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero),
            InitialFee = Math.Round(fees, 2, MidpointRounding.AwayFromZero),
            FeeCurrency = feeCurrency,
            Fee = Math.Round(fees, 2, MidpointRounding.AwayFromZero)
        };
        FifoItems.Add(item);
    }

    /// <summary>
    /// Removes a specified quantity of a sell item from the FIFO list.
    /// </summary>
    /// <param name="id">The ID of the sell item.</param>
    /// <param name="valueDate">The value date of the sell item.</param>
    /// <param name="valueTime">The value time of the sell item.</param>
    /// <param name="postDate">The post date of the sell item.</param>
    /// <param name="symbol">The symbol of the sell item.</param>
    /// <param name="quantity">The quantity of the sell item.</param>
    /// <param name="price">The price of the sell item.</param>
    /// <param name="feeCurrency">The currency of the fees.</param>
    /// <param name="fees">The fees associated with the sell item.</param>
    /// <param name="currency">The currency of the sell item.</param>
    /// <param name="amount">The total amount of the sell item.</param>
    /// <exception cref="ArgumentException">Thrown when the calculated amount does not match the provided amount.</exception>
    /// <exception cref="InvalidOperationException">Thrown when more items are sold than bought.</exception>
    public void Pop(
                    int id,
                    DateOnly valueDate,
                    TimeOnly valueTime,
                    DateOnly postDate,
                    string symbol,
                    int quantity,
                    decimal price,
                    string feeCurrency,
                    decimal fees,
                    string currency,
                    decimal amount)
    {
        if (Math.Abs(Math.Round(price * quantity - amount)) > 0.01M)
        {
            throw new ArgumentException("Cena * ilosc <> amount", nameof(price));
        }

        var i = quantity;
        var initialQuantity = quantity;
        var initialfees = Math.Round(fees, 2, MidpointRounding.AwayFromZero);
        var initialamount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        fees = initialfees;
        amount = initialamount;
        foreach (var item in FifoItems.Where(x => x.Quantity != 0 && x.Symbol == symbol).OrderBy(x => x.Id))
        {
            var cnt = item.SellItems.Count;
            FifoSellItem sellItem;

            if (quantity >= item.Quantity)
            {
                sellItem = new FifoSellItem(
                    id,
                    cnt + 1,
                    valueDate,
                    valueTime,
                    postDate,
                    item.Quantity,
                    item.Amount,
                    currency,
                    Math.Round(initialamount * quantity / initialQuantity, 2, MidpointRounding.AwayFromZero)
                        - Math.Round(initialamount * (quantity - item.Quantity) / initialQuantity, 2, MidpointRounding.AwayFromZero),
                    item.Fee,
                    feeCurrency,
                    Math.Round(initialfees * quantity / initialQuantity, 2, MidpointRounding.AwayFromZero)
                        - Math.Round(initialfees * (quantity - item.Quantity) / initialQuantity, 2, MidpointRounding.AwayFromZero)
                );

                item.SellItems.Add(sellItem);
                quantity -= item.Quantity;
                fees -= sellItem.Fee;
                amount -= sellItem.Amount;
                item.Quantity = 0;
                item.Amount = 0;
                item.Fee = 0;
            }
            else
            {
                sellItem = new FifoSellItem(
                    id,
                    cnt + 1,
                    valueDate,
                    valueTime,
                    postDate,
                    quantity,
                    Math.Round(item.InitialAmount * item.Quantity / item.InitialQuantity, 2, MidpointRounding.AwayFromZero)
                        - Math.Round(item.InitialAmount * (item.Quantity - quantity) / item.InitialQuantity, 2, MidpointRounding.AwayFromZero),
                    currency,
                    amount,
                    Math.Round(item.InitialFee * item.Quantity / item.InitialQuantity, 2, MidpointRounding.AwayFromZero)
                        - Math.Round(item.InitialFee * (item.Quantity - quantity) / item.InitialQuantity, 2, MidpointRounding.AwayFromZero),
                    feeCurrency,
                    fees
                );

                item.SellItems.Add(sellItem);
                item.Quantity -= quantity;
                item.Amount -= sellItem.BuyAmount;
                item.Fee -= sellItem.BuyFee;
                quantity = 0;
                fees = 0;
                amount = 0;
            }

            if (quantity == 0)
            {
                return;
            }
        }

        throw new InvalidOperationException("Więcej akcji sprzedano niż kupiono. Dziwne!");
    }

    /// <summary>
    /// Splits the quantity of a specified symbol in the FIFO list.
    /// </summary>
    /// <param name="symbol">The symbol of the items to split.</param>
    /// <param name="oldQuantity">The old quantity before the split.</param>
    /// <param name="newQuantity">The new quantity after the split.</param>
    /// <exception cref="ArgumentException">Thrown when the old quantity does not match the total quantity of the symbol or the new quantity is not a multiple of the old quantity.</exception>
    public void Split(
                string symbol,
                int oldQuantity,
                int newQuantity)
    {
        var cnt = FifoItems.Where(x => x.Quantity != 0 && x.Symbol == symbol).Select(x => x.Quantity).Sum();
        if (cnt != oldQuantity)
        {
            throw new ArgumentException("Nie zgadza się liczba akcji dla split", nameof(oldQuantity));
        }
        if (newQuantity % oldQuantity != 0)
        {
            throw new ArgumentException("Nowa liczba akcji nie dzieli się przez starą", nameof(newQuantity));
        }

        var k = newQuantity / oldQuantity;
        foreach (var item in FifoItems.Where(x => x.Quantity != 0 && x.Symbol == symbol).OrderBy(x => x.Id))
        {
            item.Quantity *= k;
            item.InitialQuantity *= k;
        }
    }
}