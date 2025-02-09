using System;

namespace Freedom24PolTax.Dto;

/// <summary>
/// Represents a dividend data transfer object.
/// </summary>
/// <param name="Id">The unique identifier of the dividend.</param>
/// <param name="Type">The type of the dividend.</param>
/// <param name="PostingDate">The date the dividend was posted.</param>
/// <param name="Asset">The asset associated with the dividend.</param>
/// <param name="Amount">The amount of the dividend.</param>
/// <param name="UnitValue">The unit value of the dividend.</param>
/// <param name="UnitCurrency">The currency of the unit value.</param>
/// <param name="Ticker">The ticker symbol of the asset.</param>
/// <param name="Isin">The ISIN of the asset.</param>
/// <param name="FixationDate">The fixation date of the dividend.</param>
/// <param name="Quantity">The quantity of the asset.</param>
/// <param name="TaxAmount">The tax amount on the dividend.</param>
/// <param name="TaxCurrency">The currency of the tax amount.</param>
/// <param name="Comment">Additional comments about the dividend.</param>
public record DividendDto(
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