using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DividendTax.Dto;

namespace DividendTax;

public class FileImports
{
    public static IList<DividendDto> ImportTransactions(string fileName)
    {
        CultureInfo culture = CultureInfo.GetCultureInfo("pl-PL");
        var config = new CsvConfiguration(culture);
        config.HasHeaderRecord = true;
        config.Delimiter = ";";
        using var reader = new StreamReader(fileName);
        using var csv = new CsvReader(reader, config);
        IList<RawDividendDto> records = new List<RawDividendDto>();
        csv.Read();
        csv.ReadHeader();
        int line = 1; // do wskazania w której linii pliku wejściowego jest błąd
        try
        {
            while (csv.Read())
            {
                var record = new RawDividendDto();


                record.Type = csv.GetField<string>(0);
                if (record.Type != "Dividends")
                {
                    throw new ArgumentException("Incorrect value", nameof(record.Type));
                }

                record.PostingDate = csv.GetField<DateOnly>(1);
                record.Asset = csv.GetField<string>(2);
                record.Amount = csv.GetField<decimal>(3);
                record.UnitValue = csv.GetField<decimal>(4);
                record.UnitCurrency = csv.GetField<string>(5);
                record.Ticker = csv.GetField<string>(6);
                record.Isin = csv.GetField<string>(7);
                record.FixationDate = csv.GetField<DateOnly>(8);
                record.Quantity = csv.GetField<int>(9);
                record.TaxDeductedSource = csv.GetField<string>(10);
                record.BrokerTax = csv.GetField<string>(11);
                record.Comment = csv.GetField<string>(12);

                records.Add(record);
                line++;
            }
        }
        catch (Exception e)
        {
            throw;
        }

        //records = records.Reverse().ToList();
        IList<DividendDto> dividends = new List<DividendDto>();
        int i = 0;
        foreach (var record in records)
        {
            i++;
            DividendDto dividend = new();
            dividend.Id = i;
            dividend.Type = record.Type;
            dividend.PostingDate = record.PostingDate;
            dividend.Asset = record.Asset;
            dividend.Amount = record.Amount;
            dividend.UnitValue = record.UnitValue;
            dividend.UnitCurrency = record.UnitCurrency;
            dividend.Ticker = record.Ticker;
            dividend.Isin = record.Isin;
            dividend.FixationDate = record.FixationDate;
            dividend.Quantity = record.Quantity;
            (dividend.TaxAmount, dividend.TaxCurrency) = SplitAmount(record.BrokerTax);
            dividend.Comment = record.Comment;
            dividends.Add(dividend);
        }

        return dividends;
    }

    private static (decimal, string) SplitAmount(string value)
    {
        int l = value.Length;
        string currency = value.Substring(l - 3);
        string s = value.Substring(0, l - 3);
        decimal amount = decimal.Parse(s, CultureInfo.InvariantCulture);
        return (amount, currency);
    }
}