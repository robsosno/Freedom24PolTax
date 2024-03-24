using CommonUtils;
using SharesTax;
using SharesTax.Dto;

var fileName = "d:\\documents\\freedom24\\input\\trades2_2023.csv";
IList<TransactionDto> transactions = FileImports.ImportTransactions(fileName);
var fifo = Calculations.CalculateStockProfit(transactions);
INBPApi nbpApi = new NBPApi();
Exports.StockProfit(fifo, nbpApi);