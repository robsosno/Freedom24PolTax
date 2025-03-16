using System;

namespace Freedom24PolTax.Dto;

/// <summary>
/// Represents the type of conversion.
/// </summary>
public enum ConversionType
{
    /// <summary>
    /// Represents a sell conversion.
    /// </summary>
    Sell,

    /// <summary>
    /// Represents a replacement conversion.
    /// </summary>
    Replacement
}

/// <summary>
/// Represents a component of a conversion.
/// </summary>
/// <param name="Id">The unique identifier of the conversion component.</param>
/// <param name="ConversionType">The type of the conversion.</param>
/// <param name="ActionId">Corporate action identifier associated with the conversion.</param>
/// <param name="Items">The list of conversion items.</param>
public record ConversionComponent(
    int Id,
    ConversionType ConversionType,
    string ActionId,
    IList<ConversionItem> Items
    );