using System;

namespace SharesTax.Dto;

public record FifoSellItem(
    int GroupId,
    int Id,
    DateOnly ValueDate,
    TimeOnly ValueTime,
    DateOnly PostDate,
    int Quantity,
    decimal BuyAmount,
    string Currency,
    decimal Amount,
    decimal BuyFee,
    string FeeCurrency,
    decimal Fee);