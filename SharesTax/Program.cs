using CommonUtils;
using SharesTax;
using SharesTax.Dto;

var fileName = "d:\\documents\\freedom24\\1315975_2023-05-12 23 59 59_2024-03-23 23 59 59_all.json";
IList<DividendDto> dividends;
IList<TransactionDto> transactions;
(transactions, dividends)= FileImports.ImportTransactions(fileName);
var fifo = Calculations.CalculateStockProfit(transactions);
INBPApi nbpApi = new NBPApi();
Exports.StockProfit(fifo, nbpApi);
Exports.CalculateDividendProfit(dividends, nbpApi);