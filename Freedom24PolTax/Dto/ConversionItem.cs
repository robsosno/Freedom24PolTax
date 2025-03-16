using System;

namespace Freedom24PolTax.Dto;

/// <summary>
/// Represents a conversion item with various financial details.
/// </summary>
/// <param name="Id">The unique identifier of the conversion item. Example: 12</param>
/// <param name="Type">The type of the conversion. Example: "Split"</param>
/// <param name="PostingDate">The date the conversion was posted. Example: 2024-06-17</param>
/// <param name="Asset">The asset type involved in the conversion. Example: " Securities ", " Cash "</param>
/// <param name="Amount">The amount involved in the conversion. Example: 0.68</param>
/// <param name="UnitValue">The unit value of the asset. Example: 0.6849</param>
/// <param name="UnitCurrency">The currency of the unit value. Example: "USD"</param>
/// <param name="Ticker">The ticker symbol of the asset. Example: "SPCE.US"</param>
/// <param name="Isin">The ISIN of the asset. Example: "US92766K4031"</param>
/// <param name="FixationDate">The date the conversion rate was fixed. Example: 2024-06-14</param>
/// <param name="Quantity">The quantity of the asset. Example: 1.00000000</param>
/// <param name="TaxAmount">The tax amount for the conversion. Example: 200.00</param>
/// <param name="TaxCurrency">The currency of the tax amount. Example: "USD"</param>
/// <param name="Comment">Additional comments about the conversion. Example: " Stock split SPCE.US (US92766K1060). Record date 2024-06-14, factor: 20/1. "</param>
public record ConversionItem(
    int Id,
    string Type,
    DateOnly PostingDate,
    string Asset,
    decimal Amount,
    decimal UnitValue,
    string UnitCurrency,
    string Ticker,
    string Isin,
    DateOnly FixationDate,
    int Quantity,
    decimal TaxAmount,
    string TaxCurrency,
    string Comment);