using CommonUtils;
using GNU.Getopt;
using Freedom24PolTax;
using Freedom24PolTax.Dto;

const string ProgramName = "ExportDomain";

var getopt = new Getopt(ProgramName, args, "i:s:d:h");
int c;
string inputFileName = string.Empty;
string dividendFileName = "dividends.csv";
string sharesFileName = "shares.csv";

while ((c = getopt.getopt()) != -1)
{
    switch (c)
    {
        case 'i':
            inputFileName = getopt.Optarg;
            break;
        case 'd':
            dividendFileName = getopt.Optarg;
            break;
        case 's':
            sharesFileName = getopt.Optarg;
            break;
        case 'h':
            DisplayHelp();
            return 0;
        default:
            Console.WriteLine($"getopt2() returned {c}, arg: {getopt.Optarg}, opt: {getopt.Optopt}, ind: {getopt.Optind}, longind: {getopt.Longind}");
            break;
    }
}

if (inputFileName.Length == 0)
{
    Console.WriteLine("Input file name is required");
    DisplayHelp();
    return 1;
}

IList<DividendDto> dividends;
IList<TransactionDto> transactions;
(transactions, dividends) = FileImports.ImportTransactions(inputFileName);
var fifo = Calculations.CalculateStockProfit(transactions);
INBPApi nbpApi = new NBPApi();
Exports.StockProfit(sharesFileName, fifo, nbpApi);
Exports.CalculateDividendProfit(dividendFileName, dividends, nbpApi);
return 0;

static void DisplayHelp()
{
    Console.WriteLine("{0}  version 1.0", ProgramName);
    Console.WriteLine("Copyright (c) Bank Zachodni WBK 2012");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("   -i       input file name (required)");
    Console.WriteLine("   -d       optional output dividend file name");
    Console.WriteLine("   -s       optional output shares file name");
    Console.WriteLine("   -h       display this help screen.");
}