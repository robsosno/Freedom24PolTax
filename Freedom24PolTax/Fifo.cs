using System;
using Freedom24PolTax.Dto;

namespace Freedom24PolTax;

public class Fifo
{
    public IList<FifoBuyItem> FifoItems { get; set; }

    public Fifo()
    {
        FifoItems = [];
    }

    private int maxId = 0;

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