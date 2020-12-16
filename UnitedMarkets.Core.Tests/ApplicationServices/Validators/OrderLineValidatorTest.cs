using System;
using FluentAssertions;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.Entities;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Validators
{
    public class OrderLineValidatorTest
    {
        private OrderLineValidator _olValidator;

        public OrderLineValidatorTest()
        {
            _olValidator = new OrderLineValidator();
        }

        [Fact]
        public void OrderLineValidator_ShouldBeOfTypeIOrderLineValidator()
        {
            new OrderLineValidator().Should().BeAssignableTo<IValidator<OrderLine>>();
        }

        [Theory]
        [InlineData(-4)]
        [InlineData(0)]
        public void DefaultValidation_OrderLineWithInvalidProductId_ShouldThrowException(int productId)
        {
            var orderLine = new OrderLine() {ProductId = productId, Quantity = 3, SubTotalPrice = 874.70};
            Action action = () => _olValidator.DefaultValidation(orderLine);
            action.Should().Throw<ArgumentException>()
                .WithMessage(productId == 0
                    ? ("Product id cannot be 0.")
                    : ("Product id cannot be negative value."));
        }

        [Theory]
        [InlineData(-4)]
        [InlineData(0)]
        public void DefaultValidation_OrderLineWithInvalidQuantity_ShouldThrowException(int quantity)
        {
            var orderLine = new OrderLine() {ProductId = 2, Quantity = quantity, SubTotalPrice = 874.70};
            Action action = () => _olValidator.DefaultValidation(orderLine);
            action.Should().Throw<ArgumentException>()
                .WithMessage(quantity == 0
                    ? ("Order line quantity cannot be 0.")
                    : ("Order line quantity cannot be negative value."));
        }

        [Theory]
        [InlineData(-4)]
        [InlineData(0)]
        public void DefaultValidation_OrderLineWithInvalidSubtotalPrice_ShouldThrowException(int subTotal)
        {
            var orderLine = new OrderLine() {ProductId = 2, Quantity = 7, SubTotalPrice = subTotal};
            Action action = () => _olValidator.DefaultValidation(orderLine);
            action.Should().Throw<ArgumentException>()
                .WithMessage(subTotal == 0
                    ? ("Order line sub total price cannot be 0.")
                    : ("Order line sub total price cannot be negative value."));
        }


       
        
        
        
    }
}