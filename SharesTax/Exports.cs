using CommonUtils;
using CommonUtils.Dto;
using SharesTax.Dto;

namespace SharesTax;

public class Exports
{
    public static void StockProfit(Fifo fifo, INBPApi nbpApi)
    {
        IList<StockReportItem> l = [];
        foreach (var buyItem in fifo.FifoItems.Where(x => x.SellItems.Count != 0).OrderByDescending(x => x.Id))
        {
            foreach (var sellItem in buyItem.SellItems)
            {
                StockReportItem stockReportItem = new()
                {
                    GroupId = sellItem.GroupId,
                    Id = sellItem.Id,
                    Symbol = buyItem.Symbol,
                    Isin = buyItem.Isin,
                    Quantity = sellItem.Quantity,
                    OpenValueDate = buyItem.ValueDate,
                    OpenValueTime = buyItem.ValueTime,
                    OpenPostDate = buyItem.PostDate,
                    BuyAmount = sellItem.BuyAmount,
                    BuyFee = sellItem.BuyFee,
                    CloseValueDate = sellItem.ValueDate,
                    CloseValueTime = sellItem.ValueTime,
                    ClosePostDate = sellItem.PostDate,
                    SellAmount = sellItem.SellAmount,
                    SellFee = sellItem.SellFee
                };
                l.Add(stockReportItem);
            }
        }

        using StreamWriter writetext = new("shares.csv");
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
            NBPRate buyRate = nbpApi.GetRate("USD", stockReportItem.OpenPostDate);
            NBPRate sellRate = nbpApi.GetRate("USD", stockReportItem.ClosePostDate);
            NBPRate buyFeeRate = nbpApi.GetRate("EUR", stockReportItem.OpenPostDate);
            NBPRate sellFeeRate = nbpApi.GetRate("EUR", stockReportItem.ClosePostDate);
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
                buyRate.Rate,
                buyFeeRate.Rate,
                Math.Round(stockReportItem.BuyAmount * buyRate.Rate, 2),
                Math.Round(stockReportItem.BuyFee * buyFeeRate.Rate, 2),
                stockReportItem.SellAmount,
                stockReportItem.SellFee,
                sellRate.Rate,
                sellFeeRate.Rate,
                Math.Round(stockReportItem.SellAmount * sellRate.Rate, 2),
                Math.Round(stockReportItem.SellFee * sellFeeRate.Rate, 2)
                );
        }
    }

    public static void CalculateDividendProfit(IList<DividendDto> dividends, INBPApi nbpApi)
    {
        using (StreamWriter writetext = new("dividend.csv"))
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
                NBPRate rate = nbpApi.GetRate(d.UnitCurrency, d.PostingDate);
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