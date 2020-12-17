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
        private readonly OrderLineValidator _orderLineValidator;

        public OrderLineValidatorTest()
        {
            _orderLineValidator = new OrderLineValidator();
        }

        [Fact]
        public void OrderLineValidator_ShouldBeOfTypeIOrderLineValidator()
        {
            new OrderLineValidator().Should().BeAssignableTo<IValidator<OrderLine>>();
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DefaultValidation_OrderLineWithZeroOrNegativeProductId_ShouldThrowException(int productId)
        {
            var orderLine = new OrderLine {ProductId = productId};
            Action action = () => _orderLineValidator.DefaultValidation(orderLine);
            action.Should().Throw<ArgumentException>()
                .WithMessage(productId == 0
                    ? "ProductId cannot be 0 in OrderLine. (Parameter 'ProductId')"
                    : "ProductId cannot be a negative value in OrderLine. (Parameter 'ProductId')");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DefaultValidation_OrderLineWithZeroOrNegativeQuantity_ShouldThrowException(int quantity)
        {
            var orderLine = new OrderLine {ProductId = 2, Quantity = quantity};
            Action action = () => _orderLineValidator.DefaultValidation(orderLine);
            action.Should().Throw<ArgumentException>()
                .WithMessage(quantity == 0
                    ? "Quantity cannot be 0 in OrderLine. (Parameter 'Quantity')"
                    : "Quantity cannot be a negative value in OrderLine. (Parameter 'Quantity')");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DefaultValidation_OrderLineWithZeroOrNegativeSubTotalPrice_ShouldThrowException(int subTotal)
        {
            var orderLine = new OrderLine {ProductId = 2, Quantity = 7, SubTotalPrice = subTotal};
            Action action = () => _orderLineValidator.DefaultValidation(orderLine);
            action.Should().Throw<ArgumentException>()
                .WithMessage(subTotal == 0
                    ? "SubTotalPrice cannot be 0 in OrderLine. (Parameter 'SubTotalPrice')"
                    : "SubTotalPrice cannot be a negative value in OrderLine. (Parameter 'SubTotalPrice')");
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DefaultValidation_OrderLineWithZeroOrNegativeOrderId_ShouldThrowException(int orderId)
        {
            var orderLine = new OrderLine {OrderId = orderId};
            Action action = () => _orderLineValidator.UpdateValidation(orderLine);
            action.Should().Throw<ArgumentException>()
                .WithMessage(orderId == 0
                    ? "OrderId cannot be 0 in OrderLine. (Parameter 'OrderId')"
                    : "OrderId cannot be a negative value in OrderLine. (Parameter 'OrderId')");
        }
    }
}