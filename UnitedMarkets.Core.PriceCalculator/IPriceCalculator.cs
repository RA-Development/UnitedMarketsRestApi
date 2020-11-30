using System.Collections.Generic;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.PriceCalculator
{
    public interface IPriceCalculator
    {
        Product CalculatePrice(Product product);
    }
}