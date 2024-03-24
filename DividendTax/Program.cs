using CommonUtils;
using DividendTax;
using DividendTax.Dto;

var fileName = "d:\\documents\\freedom24\\input\\dividends_2023.csv";
IList<DividendDto> dividends = FileImports.ImportTransactions(fileName);
INBPApi nbpApi = new NBPApi();
Exports.CalculateDividendProfit(dividends, nbpApi);