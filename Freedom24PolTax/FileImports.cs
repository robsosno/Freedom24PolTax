using System.Globalization;
using System.Text.Json;
using Freedom24PolTax.Dto;

namespace Freedom24PolTax;

public class FileImports
{
    /// <summary>
    /// Imports transactions and dividends from a JSON file.
    /// </summary>
    /// <param name="fileName">The path to the JSON file.</param>
    /// <returns>A tuple containing a list of transactions and a list of dividends.</returns>
    /// <exception cref="ArgumentException">Thrown when an incorrect value is encountered in the JSON data.</exception>
    public static (IList<TransactionDto>, IList<DividendDto>) ImportTransactions(string fileName)
    {
        CultureInfo culture = CultureInfo.InvariantCulture;

        using StreamReader reader = new(fileName);
        var json = reader.ReadToEnd();
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
        var dateStart = jsonElement.GetProperty("date_start").GetString();
        var dateEnd = jsonElement.GetProperty("date_end").GetString();
        var plainAccountInfoData = jsonElement.GetProperty("plainAccountInfoData");
        var base_currency = plainAccountInfoData.GetProperty("base_currency").GetString();
        var accountInfo = jsonElement.GetProperty("accountInfo");
        foreach (var info in accountInfo.EnumerateArray())
        {
            var commission_currency = info.GetProperty(" Commission currency ").GetString();
            break;
        }
        var trades = jsonElement.GetProperty("trades");
        var tradesDetailed = trades.GetProperty("detailed");
        IList<TransactionDto> transactions = [];
        int i = 1;
        foreach (var trade in tradesDetailed.EnumerateArray())
        {
            var tradeNo = trade.GetProperty("trade_id").GetInt32();
            var s = trade.GetProperty("date").GetString();
            var transactionDateTime = s != null ? DateTime.Parse(s) : DateTime.MinValue;
            s = trade.GetProperty("pay_d").GetString();
            var postingDate = s != null ? DateOnly.Parse(s) : DateOnly.MinValue;
            var ticker = trade.GetProperty("instr_nm").GetString() ?? string.Empty;
            var isin = trade.GetProperty("isin").GetString() ?? string.Empty;
            s = trade.GetProperty("operation").GetString();
            OrderType orderType;
            if (s == "sell")
            {
                orderType = OrderType.Sell;
            }
            else
            {
                if (s == "buy")
                {
                    orderType = OrderType.Buy;
                }
                else
                {
                    throw new ArgumentException("Incorrect value", nameof(orderType));
                }
            }
            var quantity = trade.GetProperty("q").GetInt32();
            var price = trade.GetProperty("p").GetDecimal();
            var amountCurrency = trade.GetProperty("curr_c").GetString() ?? string.Empty;
            var amount = trade.GetProperty("summ").GetDecimal();
            var profit = decimal.Parse(trade.GetProperty("fifo_profit").GetString() ?? "0", culture);
            var commissionCurrency = trade.GetProperty("commission_currency").GetString() ?? string.Empty;
            var commission = trade.GetProperty("commission").GetDecimal();

            var transaction = new TransactionDto(
                i,
                tradeNo.ToString(),
                DateOnly.FromDateTime(transactionDateTime),
                TimeOnly.FromDateTime(transactionDateTime),
                postingDate,
                ticker,
                isin,
                orderType,
                quantity,
                price,
                amountCurrency,
                amount,
                profit,
                commissionCurrency,
                commission);
            transactions.Add(transaction);
            i++;
        }

        var commissions = jsonElement.GetProperty("commissions");
        var commissionsTotal = commissions.GetProperty("total");
        var commissionsTotalEur = decimal.Parse(commissionsTotal.GetProperty("EUR").GetString() ?? "0", culture);

        var corporateActions = jsonElement.GetProperty("corporate_actions");
        var corporateActionsDetailed = corporateActions.GetProperty("detailed");
        IList<DividendDto> dividends = [];
        i = 0;
        foreach (var corporateAction in corporateActionsDetailed.EnumerateArray())
        {
            i++;
            var type = corporateAction.GetProperty("type").GetString() ?? string.Empty;
            if (type != "Dividends")
            {
                throw new ArgumentException("Incorrect value", nameof(type));
            }

            var s = corporateAction.GetProperty("date").GetString();
            var postingDate = s != null ? DateOnly.Parse(s) : DateOnly.MinValue;
            var asset = corporateAction.GetProperty("asset_type").GetString() ?? string.Empty;
            var amount = corporateAction.GetProperty("amount").GetDecimal();
            var unitValue = corporateAction.GetProperty("amount_per_one").GetDecimal();
            var unitCurrency = corporateAction.GetProperty("currency").GetString() ?? string.Empty;
            var ticker = corporateAction.GetProperty("ticker").GetString() ?? string.Empty;
            var isin = corporateAction.GetProperty("isin").GetString() ?? string.Empty;
            s = corporateAction.GetProperty("ex_date").GetString();
            var fixationDate = s != null ? DateOnly.Parse(s) : DateOnly.MinValue;
            s = corporateAction.GetProperty("q_on_ex_date").GetString() ?? "0";
            var quantity = Decimal.ToInt32(decimal.Parse(s, culture));
            s = corporateAction.GetProperty("tax_amount").GetString() ?? "0";
            var taxAmount = decimal.Parse(s, culture);
            var taxCurrency = corporateAction.GetProperty("tax_currency").GetString() ?? string.Empty;
            var comment = corporateAction.GetProperty("comment").GetString() ?? string.Empty;

            DividendDto dividend = new
            (
                i,
                type,
                postingDate,
                asset,
                amount,
                unitValue,
                unitCurrency,
                ticker,
                isin,
                fixationDate,
                quantity,
                taxAmount,
                taxCurrency,
                comment
            );

            dividends.Add(dividend);
            i++;
        }

        return (transactions, dividends);
    }
}