using System;
using CommonUtils.Dto;

namespace CommonUtils;

public interface INBPApi
{
    NBPRate GetRate(string billingCurrency, DateOnly valueDate);
}