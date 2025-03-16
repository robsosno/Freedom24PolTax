using System;
using Freedom24PolTax.Dto;

namespace Freedom24PolTax;

public class Calculations
{
    /// <summary>
    /// Calculates the stock profit using FIFO method based on the provided transactions and conversions.
    /// </summary>
    /// <param name="transactions">The list of transactions to process.</param>
    /// <param name="conversions">The list of conversion components to process.</param>
    /// <param name="awards">The list of awards to process.</param>
    /// <returns>A <see cref="Fifo"/> object containing the calculated stock profit.</returns>
    public static Fifo CalculateStockProfit(IList<TransactionDto> transactions, IList<ConversionComponent> conversions, IList<AwardDto> awards)
    {
        List<object> l = new();
        var trEnumerator = transactions.GetEnumerator();
        var cnvEnumerator = conversions.GetEnumerator();
        var awEnumerator = awards.GetEnumerator();
        var trExists = trEnumerator.MoveNext();
        var cnvExists = cnvEnumerator.MoveNext();
        var awExists = awEnumerator.MoveNext();
        DateOnly trDate;
        DateOnly cnvDate;
        DateOnly awDate;
        while (true)
        {
            if (!trExists && !cnvExists && !awExists)
            {
                break;
            }
            if (trExists)
            {
                trDate = trEnumerator.Current.PostingDate;
            }
            else
            {
                trDate = DateOnly.MaxValue;
            }
            if (cnvExists)
            {
                cnvDate = cnvEnumerator.Current.Items[0].PostingDate;
            }
            else
            {
                cnvDate = DateOnly.MaxValue;
            }
            if (awExists)
            {
                awDate = DateOnly.FromDateTime(awEnumerator.Current.Datetime);
            }
            else
            {
                awDate = DateOnly.MaxValue;
            }
            if (trDate < cnvDate)
            {
                if (trDate < awDate)
                {
                    l.Add(trEnumerator.Current);
                    trExists = trEnumerator.MoveNext();
                }
                else
                {
                    if (cnvDate < awDate)
                    {
                        l.Add(cnvEnumerator.Current);
                        cnvExists = cnvEnumerator.MoveNext();
                    }
                    else
                    {
                        l.Add(awEnumerator.Current);
                        awExists = awEnumerator.MoveNext();
                    }
                }
            }
            else
            {
                if (cnvDate < awDate)
                {
                    l.Add(cnvEnumerator.Current);
                    cnvExists = cnvEnumerator.MoveNext();
                }
                else
                {
                    l.Add(awEnumerator.Current);
                    awExists = awEnumerator.MoveNext();
                }
            }
        }

        Fifo fifo = new();
        foreach (var item in l)
        {
            if (item is TransactionDto transaction)
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
                    continue;
                }
                if (transaction.OrderType == OrderType.Sell)
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
                    continue;
                }
            }

            if (item is ConversionComponent conversion)
            {
                if (conversion.ConversionType == ConversionType.Sell)
                {
                    int cash = -1;
                    int securities = -1;
                    for (int i = 0; i < conversion.Items.Count; i++)
                    {
                        if (conversion.Items[i].Asset.Contains("Cash"))
                        {
                            cash = i;
                        }
                        if (conversion.Items[i].Asset.Contains("Securities"))
                        {
                            securities = i;
                        }
                    }
                    if (cash == -1 || securities == -1)
                    {
                        throw new InvalidOperationException("Conversion items must contain both Cash and Securities.");
                    }

                    fifo.Pop(
                        conversion.Id,
                        conversion.Items[0].FixationDate,
                        TimeOnly.MinValue,
                        conversion.Items[0].PostingDate,
                        conversion.Items[0].Ticker,
                        (int)-conversion.Items[securities].Amount,
                        conversion.Items[securities].UnitValue,
                        conversion.Items[securities].UnitCurrency,
                        0,
                        conversion.Items[securities].UnitCurrency,
                        conversion.Items[cash].Amount);
                    continue;
                }
            }

            if (item is AwardDto award)
            {
                fifo.Push(
                    DateOnly.FromDateTime(award.Datetime),
                    TimeOnly.MinValue,
                    DateOnly.FromDateTime(award.Datetime),
                    award.Ticker,
                    string.Empty,
                    award.Quantity,
                    0,
                    string.Empty,
                    0,
                    string.Empty,
                    0);
                continue;

            }
        }

        return fifo;
    }
}