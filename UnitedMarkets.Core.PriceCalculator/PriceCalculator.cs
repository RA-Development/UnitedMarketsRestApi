using System;
using System.Collections.Generic;
using System.Globalization;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.PriceCalculator
{
    public class PriceCalculator : IPriceCalculator
    {
        public Product CalculatePrice(Product product)
        {
            var p = product;
            var price = p.Amount * p.PricePerUnit;
            p.Price = Math.Round(price, 2);
            return p;
        }
    }
}