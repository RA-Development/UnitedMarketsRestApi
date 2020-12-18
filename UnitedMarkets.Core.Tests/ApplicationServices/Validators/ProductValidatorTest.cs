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
        private readonly IValidator<Product> _productValidator;

        public ProductValidatorTest()
        {
            _productValidator = new ProductValidator();
        }

        [Fact]
        public void ProductValidator_ShouldBeOfTypeIValidatorProduct()
        {
            new ProductValidator().Should().BeAssignableTo<IValidator<Product>>();
        }

        [Fact]
        public void DefaultValidation_ProductWithNegativePricePerUnit_ThrowsArgumentException()
        {
            var product = new Product
            {
                PricePerUnit = -7.00
            };

            Action action = () => _productValidator.DefaultValidation(product);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Positive value required for product price. (Parameter 'product')");
        }

        [Fact]
        public void DefaultValidation_ProductWithNegativeAmount_ThrowsArgumentException()
        {
            var product = new Product
            {
                PricePerUnit = 7.00,
                Amount = -2
            };

            Action action = () => _productValidator.DefaultValidation(product);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Positive value required for product amount. (Parameter 'product')");
        }
    }
}