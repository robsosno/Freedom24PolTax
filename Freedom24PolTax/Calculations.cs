using System;
using Freedom24PolTax.Dto;

namespace Freedom24PolTax;

public class Calculations
{
    /// <summary>
    /// Calculates the stock profit using FIFO (First In, First Out) method.
    /// </summary>
    /// <param name="transactions">The list of transactions to process.</param>
    /// <returns>A <see cref="Fifo"/> object containing the processed transactions.</returns>
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