using System;
using FluentAssertions;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.Entities;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Validators
{
    public class ProductValidatorTest
    {
        private readonly AmountUnit _kgAmount;
        private readonly ProductValidator _productValidator;

        [Fact]
        public void ProductValidator_ShouldBeOfTypeIProductValidator()
        {
            new ProductValidator().Should().BeAssignableTo<IValidator<Product>>();
        }


        public ProductValidatorTest()
        {
            _kgAmount = new AmountUnit() {Id = 2, Name = "kg"};
            _productValidator = new ProductValidator();
        }


        [Fact]
        public void DefaultValidation_ProductWithNegativePricePerUnit_ThrowsArgumentException()
        {
            var product = new Product()
            {
                Id = 2,
                Name = "Sugar",
                CategoryId = 1,
                MarketId = 1,
                OriginId = 1,
                PricePerUnit = -7.00,
                Amount = 2,
                AmountUnit = _kgAmount,
                AmountUnitId = 2,
            };

            Action action = () => _productValidator.DefaultValidation(product);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Positive value required for product price.");
        }


        [Fact]
        public void DefaultValidation_ProductWithNegativeAmount_ThrowsArgumentException()
        {
            var product = new Product()
            {
                Id = 2,
                Name = "Sugar",
                CategoryId = 1,
                MarketId = 1,
                OriginId = 1,
                PricePerUnit = 7.00,
                Amount = -2,
                AmountUnit = _kgAmount,
                AmountUnitId = 2,
            };

            Action action = () => _productValidator.DefaultValidation(product);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Positive value required for product amount.");
        }
    }
}