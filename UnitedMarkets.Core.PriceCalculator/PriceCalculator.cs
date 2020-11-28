using System;
using System.Collections.Generic;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.PriceCalculator
{
    public class PriceCalculator : IPriceCalculator
    {
        public Product CalculatePrice(Product product)
        {
            var p = product;
            var price = p.Amount * p.PricePerUnit;
            var formatted = Math.Truncate(price * 100) / 100;
            p.Price = formatted;
            return p;
        }
    }
}