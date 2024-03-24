using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SharesTax.Dto;

namespace SharesTax;

public class FileImports
{
    public static IList<TransactionDto> ImportTransactions(string fileName)
    {
        CultureInfo culture = CultureInfo.GetCultureInfo("pl-PL");
        var config = new CsvConfiguration(culture);
        config.HasHeaderRecord = true;
        config.Delimiter = ";";
        using var reader = new StreamReader(fileName);
        using var csv = new CsvReader(reader, config);
        IList<TransactionDto> records = [];
        csv.Read();
        csv.ReadHeader();
        int line = 1;
        while (csv.Read())
        {
            string tradeNo = csv.GetField<string>(0) ?? string.Empty;
            var s = csv.GetField<string>(1);
            var transactionTime = s != null ? DateTime.Parse(s) : DateTime.MinValue;
            s = csv.GetField<string>(2);
            var postingDate = s != null ? DateOnly.Parse(s) : DateOnly.MinValue;
            string ticker = csv.GetField<string>(3) ?? string.Empty;
            s = csv.GetField<string>(4);
            OrderType orderType;
            if (s == "Sell")
            {
                orderType = OrderType.Sell;
            }
            else
            {
                if (s == "Buy")
                {
                    orderType = OrderType.Buy;
                }
                else
                {
                    throw new ArgumentException("Incorrect value", nameof(orderType));
                }
            }

            var quantity = csv.GetField<int>(5);
            var price = csv.GetField<decimal>(6);
            string amountCurrency = csv.GetField<string>(7) ?? string.Empty;
            var amount = csv.GetField<decimal>(8);
            string profitCurrency = csv.GetField<string>(9) ?? string.Empty;
            var profit = csv.GetField<decimal>(10);
            string commissionCurrency = csv.GetField<string>(11) ?? string.Empty;
            var commission = csv.GetField<decimal>(12);

            var record = new TransactionDto()
            {
                Id = 0,
                TradeNo = tradeNo,
                TransactionTime = transactionTime,
                PostingDate = postingDate,
                Ticker = ticker,
                OrderType = orderType,
                Quantity = quantity,
                Price = price,
                AmountCurrency = amountCurrency,
                Amount = amount,
                ProfitCurrency = profitCurrency,
                Profit = profit,
                CommissionCurrency = commissionCurrency,
                Commission = commission
            };

            records.Add(record);
            line++;
        }

        records = records.Reverse().ToList();
        int i = 0;
        foreach (var record in records)
        {
            i++;
            record.Id = i;
        }

        return records;
    }
}