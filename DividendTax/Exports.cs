using CommonUtils;
using CommonUtils.Dto;
using DividendTax.Dto;

namespace DividendTax;

public class Exports
{
    public static void CalculateDividendProfit(IList<DividendDto> dividends, INBPApi nbpApi)
    {
        using (StreamWriter writetext = new("dividend.csv"))
        {
            writetext.WriteLine(
                        "{0};{1};{2};{3};{4};{5};{6}",
                        "Data",
                        "Symbol",
                        "kwota",
                        "podatek",
                        "kurs",
                        "kwota PLN",
                        "podatek PLN");

            foreach (var d in dividends.OrderBy(o => o.Id))
            {
                NBPRate rate = nbpApi.GetRate(d.UnitCurrency, d.PostingDate);
                writetext.WriteLine(
                    "{0};{1};{2};{3};{4};{5};{6}",
                    d.PostingDate.ToString("dd-MM-yyyy"),
                    d.Ticker,
                    d.Amount,
                    -d.TaxAmount,
                    rate.Rate,
                    Math.Round(rate.Rate * d.Amount, 2),
                    Math.Round(rate.Rate * -d.TaxAmount, 2));
            }
        }
    }
}