using System;
using System.Diagnostics.Eventing.Reader;
using CommonUtils;
using CommonUtils.Dto;
using Freedom24PolTax.Dto;

namespace Freedom24PolTax;

public class Exports
{
    /// <summary>
    /// Generates a stock profit report and writes it to a file.
    /// </summary>
    /// <param name="sharesFileName">The name of the file to write the report to.</param>
    /// <param name="fifo">The FIFO inventory management system containing buy and sell items.</param>
    /// <param name="nbpApi">The NBP API to get currency rates.</param>
    public static void StockProfit(string sharesFileName, Fifo fifo, INBPApi nbpApi)
    {
        IList<StockReportItem> l = [];
        foreach (var buyItem in fifo.FifoItems.Where(x => x.SellItems.Count != 0).OrderByDescending(x => x.Id))
        {
            foreach (var sellItem in buyItem.SellItems)
            {
                StockReportItem stockReportItem = new
                (
                     sellItem.GroupId,
                     sellItem.Id,
                     buyItem.Symbol,
                     buyItem.Isin,
                     sellItem.Quantity,
                     buyItem.ValueDate,
                     buyItem.ValueTime,
                     buyItem.PostDate,
                     buyItem.Currency,
                     sellItem.BuyAmount,
                     buyItem.FeeCurrency,
                     sellItem.BuyFee,
                     sellItem.ValueDate,
                     sellItem.ValueTime,
                     sellItem.PostDate,
                     sellItem.Currency,
                     sellItem.Amount,
                     sellItem.FeeCurrency,
                     sellItem.Fee
                );

                l.Add(stockReportItem);
            }
        }

        using StreamWriter writetext = new(sharesFileName);
        writetext.WriteLine(
                   "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20}",
                   "Data tr.",
                   "Czas tr.",
                   "Data ksieg zakupu",
                   "Data tr.",
                   "Czas tr.",
                   "Data ksieg sprzed",
                   "Symbol",
                   "ISIN",
                   "ilość",
                   "kwota USD zakup",
                   "opłata EUR zakup",
                   "kurs USD",
                   "kurs EUR",
                   "kwota PLN zakup",
                   "opłata PLN zakup",
                   "kwota USD sprzed",
                   "opłata EUR sprzed",
                   "kurs USD",
                   "kurs EUR",
                   "kwota PLN sprzed",
                   "opłata PLN sprzed");

        foreach (var stockReportItem in l.OrderBy(x => x.GroupId).ThenBy(x => x.Id))
        {
            decimal buyRate;
            decimal sellRate;
            decimal buyFeeRate;
            decimal sellFeeRate;
            decimal buyAmountPLN;
            decimal buyFeePLN;
            decimal sellAmountPLN;
            decimal sellFeePLN;

            if (stockReportItem.BuyAmount != 0)
            {
                buyRate = nbpApi.GetRate(stockReportItem.BuyCurrency, stockReportItem.OpenPostDate).Rate;
                buyAmountPLN = stockReportItem.BuyAmount * buyRate;
            }
            else
            {
                buyRate = 1;
                buyAmountPLN = 0;
            }
            if (stockReportItem.BuyFee != 0)
            {
                buyFeeRate = nbpApi.GetRate(stockReportItem.BuyFeeCurrency, stockReportItem.OpenPostDate).Rate;
                buyFeePLN = stockReportItem.BuyFee * buyRate;
            }
            else
            {
                buyFeeRate = 1;
                buyFeePLN = 0;
            }
            if (stockReportItem.SellAmount != 0)
            {
                sellRate = nbpApi.GetRate(stockReportItem.SellCurrency, stockReportItem.ClosePostDate).Rate;
                sellAmountPLN = stockReportItem.SellAmount * sellRate;
            }
            else
            {
                sellRate = 1;
                sellAmountPLN = 0;
            }
            if (stockReportItem.SellFee != 0)
            {
                sellFeeRate = nbpApi.GetRate(stockReportItem.SellFeeCurrency, stockReportItem.ClosePostDate).Rate;
                sellFeePLN = stockReportItem.SellFee * sellRate;
            }
            else
            {
                sellFeeRate = 1;
                sellFeePLN = 0;
            }

            writetext.WriteLine(
                "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20}",
                stockReportItem.OpenValueDate.ToString("dd-MM-yyyy"),
                stockReportItem.OpenValueTime.ToString("HH:mm:ss"),
                stockReportItem.OpenPostDate.ToString("dd-MM-yyyy"),
                stockReportItem.CloseValueDate.ToString("dd-MM-yyyy"),
                stockReportItem.CloseValueTime.ToString("hh:mm:ss"),
                stockReportItem.ClosePostDate.ToString("dd-MM-yyyy"),
                stockReportItem.Symbol,
                stockReportItem.Isin,
                stockReportItem.Quantity,
                stockReportItem.BuyAmount,
                stockReportItem.BuyFee,
                buyRate,
                buyFeeRate,
                Math.Round(buyAmountPLN * buyRate, 2),
                Math.Round(buyFeePLN * buyFeeRate, 2),
                stockReportItem.SellAmount,
                stockReportItem.SellFee,
                sellRate,
                sellFeeRate,
                Math.Round(sellAmountPLN * sellRate, 2),
                Math.Round(sellFeePLN * sellFeeRate, 2)
            );
        }
    }

    /// <summary>
    /// Calculates dividend profit and writes the report to a file.
    /// </summary>
    /// <param name="dividendFileName">The name of the file to write the report to.</param>
    /// <param name="dividends">The list of dividends to calculate profit for.</param>
    /// <param name="nbpApi">The NBP API to get currency rates.</param>
    public static void CalculateDividendProfit(string dividendFileName, IList<DividendDto> dividends, INBPApi nbpApi)
    {
        using (StreamWriter writetext = new(dividendFileName))
        {
            writetext.WriteLine(
                        "{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                        "Data waluty",
                        "Data ksiegowania",
                        "Symbol",
                        "Isin",
                        "kwota",
                        "podatek",
                        "kurs",
                        "kwota PLN",
                        "podatek PLN");

            foreach (var d in dividends.OrderBy(o => o.Id))
            {
                var rate = nbpApi.GetRate(d.UnitCurrency, d.PostingDate);
                writetext.WriteLine(
                    "{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                    d.FixationDate.ToString("dd-MM-yyyy"),
                    d.PostingDate.ToString("dd-MM-yyyy"),
                    d.Ticker,
                    d.Isin,
                    d.Amount,
                    -d.TaxAmount,
                    rate.Rate,
                    Math.Round(rate.Rate * d.Amount, 2),
                    Math.Round(rate.Rate * -d.TaxAmount, 2));
            }
        }
    }
}