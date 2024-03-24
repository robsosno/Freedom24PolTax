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
                    Quantity = sellItem.Quantity,
                    OpenDate = buyItem.OpenDate,
                    BuyAmount = sellItem.BuyAmount,
                    BuyFee = sellItem.BuyFee,
                    CloseDate = sellItem.CloseDate,
                    SellAmount = sellItem.SellAmount,
                    SellFee = sellItem.SellFee
                };
                l.Add(stockReportItem);
            }
        }

        using StreamWriter writetext = new("shares.csv");
        writetext.WriteLine(
                   "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15}",
                   "Data ksieg zakupu",
                   "Data ksieg sprzed",
                   "Symbol",
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
            NBPRate buyRate = nbpApi.GetRate("USD", stockReportItem.OpenDate);
            NBPRate sellRate = nbpApi.GetRate("USD", stockReportItem.CloseDate);
            NBPRate buyFeeRate = nbpApi.GetRate("EUR", stockReportItem.OpenDate);
            NBPRate sellFeeRate = nbpApi.GetRate("EUR", stockReportItem.CloseDate);
            writetext.WriteLine(
                "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15}",
                stockReportItem.OpenDate.ToString("dd-MM-yyyy"),
                stockReportItem.CloseDate.ToString("dd-MM-yyyy"),
                stockReportItem.Symbol,
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
}