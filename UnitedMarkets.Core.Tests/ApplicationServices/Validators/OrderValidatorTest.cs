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
        public static TheoryData<DateTime> DateTimeTestMemberData = new TheoryData<DateTime>
        {
            DateTime.MinValue,
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
        [InlineData(0)]
        [InlineData(-1)]
        public void DefaultValidation_WithZeroOrNegativeOrderStatusIdInOrderParam_ShouldThrowException(int statusId)
        {
            //    Arrange
            var order = new Order {OrderStatusId = statusId};
            //    Act
            Action action = () => _orderValidator.DefaultValidation(order);
            //    Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("OrderStatusId cannot be less than 1. (Parameter 'OrderStatus')");
        }

        [Theory]
        [MemberData(nameof(ProductListTestMemberData))]
        public void DefaultValidation_WithNullOrEmptyProductListInOrderParam_ShouldThrowException(
            List<OrderLine> products)
        {
            var order = new Order {OrderStatusId = 3, Products = products};
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage(products == null
                    ? "Order must contain a list of products. (Parameter 'Products')"
                    : "List of products must contain at least one product. (Parameter 'Products')");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-7)]
        public void DefaultValidation_WithZeroOrNegativeTotalPriceInOrderParam_ShouldThrowException(double totalPrice)
        {
            //    Arrange
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine {ProductId = 2, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order
            {
                OrderStatusId = 2,
                Products = new List<OrderLine> {orderLine1, orderLine2},
                TotalPrice = totalPrice
            };
            //    Act
            Action action = () => _orderValidator.DefaultValidation(order);
            //    Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage(totalPrice == 0
                    ? "Order cannot have a total price of 0. (Parameter 'TotalPrice')"
                    : "Order cannot have a total price of negative value. (Parameter 'TotalPrice')");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DefaultValidation_WithNullOrEmptyBillingAddressInOrderParam_ShouldThrowException(
            string billingAddress)
        {
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine {ProductId = 2, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order
            {
                OrderStatusId = 2,
                Products = new List<OrderLine> {orderLine1, orderLine2},
                TotalPrice = 74.30,
                BillingAddress = billingAddress
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order has to contain a billing address. (Parameter 'BillingAddress')");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DefaultValidation_WithNullOrEmptyShippingAddressInOrderParam_ShouldThrowException(
            string shippingAddress)
        {
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order
            {
                OrderStatusId = 2,
                Products = new List<OrderLine> {orderLine1, orderLine2},
                TotalPrice = 74.30,
                BillingAddress = "Esbjerg 8",
                ShippingAddress = shippingAddress
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order has to contain a shipping address. (Parameter 'ShippingAddress')");
        }

        [Fact]
        public void DefaultValidation_WithDuplicateProductIdsInProductsInOrderParam_ShouldThrowException()
        {
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine {ProductId = 1, Quantity = 22, SubTotalPrice = 11};

            var order = new Order
            {
                OrderStatusId = 2,
                Products = new List<OrderLine> {orderLine1, orderLine2},
                TotalPrice = 7.80,
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8"
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("ProductId in each order line has to be unique. (Parameter 'orderProducts')");
        }

        [Fact]
        public void IdValidation_WithIdParamLessThan1_ShouldThrowException()
        {
            const int id = 0;
            Action action = () => _orderValidator.IdValidation(id);
            action.Should().Throw<ArgumentException>().WithMessage("Id cannot be less than 1. (Parameter 'id')");
        }

        [Theory]
        [MemberData(nameof(DateTimeTestMemberData))]
        public void CreateValidation_WithNullOrPastOrFutureDateCreatedInOrderParam_ShouldThrowException(
            DateTime dateTime)
        {
            var order = new Order {DateCreated = dateTime};
            Action action = () => _orderValidator.CreateValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("DateCreated for Order has to be within 5 seconds' precision. (Parameter 'dateCreated')");
        }

        [Theory]
        [MemberData(nameof(DateTimeTestMemberData))]
        public void CreateValidation_WithNullOrPastOrFutureDateUpdatedInOrderParam_ShouldThrowException(
            DateTime dateTime)
        {
            var order = new Order {DateCreated = DateTime.Now, DateUpdated = dateTime};
            Action action = () => _orderValidator.CreateValidation(order);
            action.Should().Throw<ArgumentException>().WithMessage(
                "DateUpdated for Order has to be within 5 seconds' precision. (Parameter 'dateUpdated')");
        }

        [Fact]
        public void CreateValidation_WithoutPendingStatusInOrderParam_ShouldThrowException()
        {
            var order = new Order {DateCreated = DateTime.Now, DateUpdated = DateTime.Now};
            Action action = () => _orderValidator.CreateValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order status has to be 'pending', with id = 1,  on creation. (Parameter 'OrderStatusId')");
        }

        [Fact]
        public void UpdateValidation_WithNullDateCreatedInOrderParam_ShouldThrowException()
        {
            var order = new Order();
            Action action = () => _orderValidator.UpdateValidation(order);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Order must have a DateCreated. (Parameter 'dateCreated')");
        }

        [Theory]
        [MemberData(nameof(DateTimeTestMemberData))]
        public void UpdateValidation_WithPastOrFutureDateUpdatedInOrderParam_ShouldThrowException(DateTime dateTime)
        {
            var order = new Order {DateCreated = DateTime.Now.AddDays(-12),DateUpdated = dateTime};
            Action action = () => _orderValidator.UpdateValidation(order);
            action.Should().Throw<ArgumentException>().WithMessage(
                "Order has to contain the current date within 5 seconds' precision for DateUpdated. (Parameter 'dateUpdated')");
        }

        [Fact]
        public void UpdateValidation_WithIncorrectOrderIdReferenceInOrderLinesInOrderParam_ShouldThrowException()
        {
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70, OrderId = 4};
            var orderLine2 = new OrderLine {ProductId = 2, Quantity = 22, SubTotalPrice = 11, OrderId = 2};

            var order = new Order
            {
                Id = 2,
                Products = new List<OrderLine> {orderLine1, orderLine2},
                DateCreated = DateTime.Now.AddDays(-12),
                DateUpdated = DateTime.Now
            };
            Action action = () => _orderValidator.UpdateValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("OrderId of each OrderLine has to match with Id of Order. (Parameter 'order')");
        }
    }
}