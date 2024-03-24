using System;
using SharesTax.Dto;

namespace SharesTax;

public class Fifo
{
    public IList<FifoBuyItem> FifoItems { get; set; }

    public Fifo()
    {
        FifoItems = [];
    }

    private int maxId = 0;

    public void Push(
                    DateOnly dateTime,
                    string symbol,
                    int quantity,
                    decimal price,
                    decimal fees,
                    decimal amount)
    {
        if (Math.Abs(Math.Round(price * quantity - amount)) > 0.01M)
        {
            throw new ArgumentException("Cena * ilosc <> amount", nameof(price));
        }

        FifoBuyItem item = new()
        {
            Id = maxId++,
            OpenDate = dateTime,
            Symbol = symbol,
            InitialQuantity = quantity,
            Quantity = quantity,
            InitialBuyAmount = Math.Round(amount, 2, MidpointRounding.AwayFromZero),
            BuyAmount = Math.Round(amount, 2, MidpointRounding.AwayFromZero),
            InitialBuyFee = Math.Round(fees, 2, MidpointRounding.AwayFromZero),
            BuyFee = Math.Round(fees, 2, MidpointRounding.AwayFromZero)
        };
        FifoItems.Add(item);
    }

    public void Pop(
                    int id,
                    DateOnly dateTime,
                    string symbol,
                    int quantity,
                    decimal price,
                    decimal fees,
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
            FifoSellItem sellItem = new();
            sellItem.GroupId = id;
            sellItem.Id = cnt + 1;
            sellItem.CloseDate = dateTime;
            if (quantity >= item.Quantity)
            {
                sellItem.Quantity = item.Quantity;
                sellItem.BuyAmount = item.BuyAmount;
                sellItem.SellAmount = Math.Round(initialamount * quantity / initialQuantity, 2, MidpointRounding.AwayFromZero)
                    - Math.Round(initialamount * (quantity - item.Quantity) / initialQuantity, 2, MidpointRounding.AwayFromZero);
                sellItem.BuyFee = item.BuyFee;
                sellItem.SellFee = Math.Round(initialfees * quantity / initialQuantity, 2, MidpointRounding.AwayFromZero)
                    - Math.Round(initialfees * (quantity - item.Quantity) / initialQuantity, 2, MidpointRounding.AwayFromZero);
                item.SellItems.Add(sellItem);
                quantity -= item.Quantity;
                fees -= sellItem.SellFee;
                amount -= sellItem.SellAmount;
                item.Quantity = 0;
                item.BuyAmount = 0;
                item.BuyFee = 0;
            }
            else
            {
                sellItem.Quantity = quantity;
                sellItem.BuyAmount = Math.Round(item.InitialBuyAmount * item.Quantity / item.InitialQuantity, 2, MidpointRounding.AwayFromZero)
                    - Math.Round(item.InitialBuyAmount * (item.Quantity - quantity) / item.InitialQuantity, 2, MidpointRounding.AwayFromZero);
                sellItem.SellAmount = amount;
                sellItem.BuyFee = Math.Round(item.InitialBuyFee * item.Quantity / item.InitialQuantity, 2, MidpointRounding.AwayFromZero)
                    - Math.Round(item.InitialBuyFee * (item.Quantity - quantity) / item.InitialQuantity, 2, MidpointRounding.AwayFromZero);
                sellItem.SellFee = fees;
                item.SellItems.Add(sellItem);
                item.Quantity -= quantity;
                item.BuyAmount -= sellItem.BuyAmount;
                item.BuyFee -= sellItem.BuyFee;
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