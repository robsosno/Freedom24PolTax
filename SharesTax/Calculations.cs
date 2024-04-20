using System;

namespace SharesTax.Dto;

public class Calculations
{
    public static Fifo CalculateStockProfit(IList<TransactionDto> transactions)
    {
        Fifo fifo = new();
        foreach (TransactionDto transaction in transactions.Where(x => x.OrderType == OrderType.Buy || x.OrderType == OrderType.Sell).OrderBy(x => x.Id))
        {
            if (transaction.OrderType == OrderType.Buy)
            {
                fifo.Push(
                    transaction.TransactionDate,
                    transaction.TransactionTime,
                    transaction.PostingDate,
                    transaction.Ticker,
                    transaction.Isin,
                    transaction.Quantity,
                    transaction.Price,
                    transaction.CommissionCurrency,
                    transaction.Commission,
                    transaction.AmountCurrency,
                    transaction.Amount);
            }
            else
            {
                fifo.Pop(
                    transaction.Id,
                    transaction.TransactionDate,
                    transaction.TransactionTime,
                    transaction.PostingDate,
                    transaction.Ticker,
                    transaction.Quantity,
                    transaction.Price,
                    transaction.CommissionCurrency,
                    transaction.Commission,
                    transaction.AmountCurrency,
                    transaction.Amount);
            }
        }

        return fifo;
    }
}