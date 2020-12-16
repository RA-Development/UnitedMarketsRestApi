using System;
using System.Collections.Generic;
using FluentAssertions;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.Entities;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Validators
{
    public class OrderValidatorTest
    {
        public static TheoryData<DateTime> DateCreatedTestMemberData = new TheoryData<DateTime>
        {
            DateTime.Now.AddSeconds(-7),
            DateTime.Now.AddSeconds(7)
        };

        public static TheoryData<List<OrderLine>> ProductListTestMemberData = new TheoryData<List<OrderLine>>
        {
            null,
            new List<OrderLine>()
        };

        private readonly IValidatorExtended<Order> _orderValidator;

        public OrderValidatorTest()
        {
            _orderValidator = new OrderValidator();
        }

        [Fact]
        public void OrderValidator_ShouldBeOfTypeIValidatorExtendedOrder()
        {
            new OrderValidator().Should().BeAssignableTo<IValidatorExtended<Order>>();
        }

        [Fact]
        public void DefaultValidation_WithOrderParamAsNull_ShouldThrowException()
        {
            Action action = () => _orderValidator.DefaultValidation(null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Order cannot be null. (Parameter 'order')");
        }

        [Theory]
        [MemberData(nameof(ProductListTestMemberData))]
        public void DefaultValidation_WithNullOrEmptyProductListInOrderParam_ShouldThrowException(List<OrderLine> products)
        {
            var order = new Order
            {
                Products = products
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage(products == null
                    ? "Order must contain product list. (Parameter 'Products')"
                    : "Product list must contain at least one product.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DefaultValidation_WithNullOrEmptyBillingAddressInOrderParam_ShouldThrowException(string billingAddress)
        {
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order
            {
                Products = new List<OrderLine> {orderLine1, orderLine2},
                TotalPrice = 74.30,
                BillingAddress = billingAddress
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order has to contain billing address.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DefaultValidation_WithNullOrEmptyShippingAddressInOrderParam_ShouldThrowException(string shippingAddress)
        {
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order
            {
                Products = new List<OrderLine> {orderLine1, orderLine2},
                TotalPrice = 74.30,
                BillingAddress = "Esbjerg 8",
                ShippingAddress = shippingAddress
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order has to contain shipping address.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-7)]
        public void DefaultValidation_WithZeroOrNegativeTotalPriceInOrderParam_ShouldThrowException(double totalPrice)
        {
            //    Arrange
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order
            {
                Products = new List<OrderLine> {orderLine1, orderLine2},
                TotalPrice = totalPrice
            };
            //    Act
            Action action = () => _orderValidator.DefaultValidation(order);
            //    Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage(totalPrice == 0
                    ? "Order total price cannot be 0."
                    : "Order total price cannot be negative value.");
        }

        [Theory]
        [MemberData(nameof(DateCreatedTestMemberData))]
        public void DefaultValidation_WithPastOrFutureDateCreatedInOrderParam_ShouldThrowException(DateTime dateTime)
        {
            var order = new Order {DateCreated = dateTime};
            Action action = () => _orderValidator.DateCreatedValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order creation date is not within 5 second precision.");
        }

        [Fact]
        public void IdValidation_WithIdParamLessThan1_ShouldThrowException()
        {
            const int id = 0;
            Action action = () => _orderValidator.IdValidation(id);
            action.Should().Throw<ArgumentException>().WithMessage("Id cannot be less than 1.");
        }

        public static TheoryData<Order,string> OrderStatusTestMemberData = new TheoryData<Order,string>
        {
            {new Order(), "Pending"},
            {new Order {OrderStatus = new OrderStatus {Id = 1, Name = "Pending"}},"Deleted"}
        };
        
        [Theory]
        [MemberData(nameof(OrderStatusTestMemberData))]
        public void StatusValidation_WithNullOrUnexpectedOrderStatusInOrderParam_ShouldThrowException(Order order, string requiredStatus)
        {
            Action action = () => _orderValidator.StatusValidation(order, requiredStatus);
            
            if(order.OrderStatus == null)
                action.Should().Throw<ArgumentNullException>()
                    .WithMessage("Status cannot be null. (Parameter 'OrderStatus')");
            else
                action.Should().Throw<ArgumentException>().WithMessage("Status should be \"Deleted\".");
        }
    }
}