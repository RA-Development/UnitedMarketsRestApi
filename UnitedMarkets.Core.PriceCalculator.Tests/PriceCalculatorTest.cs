using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using Xunit;

namespace UnitedMarkets.Core.PriceCalculator.Tests
{
    public class PriceCalculatorTest
    {
        [Fact]
        public void CalculateProductPrice_ProductWithValidProps_ReturnsProductWithCalculatedPrice()
        {
            var calc = new PriceCalculator();
            var kgAmount = new AmountUnit() {Id = 2, Name = "kg"};
            var prodInParam = new Product()
            {
                Id = 2,
                Name = "Sugar",
                CategoryId = 1,
                MarketId = 1,
                OriginId = 1,
                PricePerUnit = 7.00,
                Amount = 2,
                AmountUnit = kgAmount,
                AmountUnitId = 2,
            };
            var expectedProduct = new Product()
            {
                Id = 2,
                Name = "Sugar",
                CategoryId = 1,
                MarketId = 1,
                OriginId = 1,
                PricePerUnit = 7.00,
                Amount = 2,
                AmountUnit = kgAmount,
                AmountUnitId = 2,
                Price = 14    //    calculated price
            };
            var result = calc.CalculatePrice(prodInParam);
            result.Price.Should().Be(expectedProduct.Price);
        }
    }
}