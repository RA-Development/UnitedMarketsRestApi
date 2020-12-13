using System;
using System.Collections.Generic;
using FluentAssertions;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.Entities;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Validators
{
    public class OrderValidatorTest
    {
        private readonly OrderValidator _orderValidator;

        public OrderValidatorTest()
        {
            _orderValidator = new OrderValidator();
        }


        [Fact]
        public void DefaultValidation_OrderInParamsAsNull_ShouldThrowException()
        {
            Action action = () => _orderValidator.DefaultValidation(null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Order Cannot be Null. (Parameter 'order')");
        }


        [Fact]
        public void DefaultValidation_OrderWithEmptyProductList_ShouldThrowException()
        {
            var order = new Order()
            {
                Products = new List<OrderLine>(),
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 15.85,
                OrderStatusId = 4,
                DateCreated = DateTime.Now
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Product list must contain at least one product.");
        }


        [Fact]
        public void DefaultValidation_OrderWithNullProductList_ShouldThrowException()
        {
            var order = new Order()
            {
                Products = null,
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 15.85,
                OrderStatusId = 4,
                DateCreated = DateTime.Now
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Order must contain product list. (Parameter 'Products')");
        }


        [Fact]
        public void DefaultValidation_OrderWithEmptyBillingAddress_ShouldThrowException()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                //    empty Billing Address
                TotalPrice = 74.30,
                OrderStatusId = 4,
                DateCreated = DateTime.Now
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order has to contain billing address.");
        }

        [Fact]
        public void DefaultValidation_OrderWithEmptyAddress_ShouldThrowException()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                // ShippingAddress is empty
                BillingAddress = "Esbjerg 8",
                TotalPrice = 74.30,
                OrderStatusId = 4,
                DateCreated = DateTime.Now
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order has to contain shipping address.");
        }

        [Fact]
        public void DefaultValidation_OrderWithoutPendingStatus_ShouldThrowException()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                BillingAddress = "Esbjerg 8",
                TotalPrice = 74.30,
                DateCreated = DateTime.Now
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order status has to be 'pending' on creation. (Pending status id = 4)");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-7)]
        public void DefaultValidation_OrderWithInvalidTotalPrice_ShouldThrowException(double totalPrice)
        {
            //    Arange
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = totalPrice,
                OrderStatusId = 4,
                DateCreated = DateTime.Now
            };
            //    Act
            Action action = () => _orderValidator.DefaultValidation(order);
            //    Assert
            action.Should().Throw<ArgumentException>()
                .WithMessage(totalPrice == 0
                    ? ("Order total price Cannot be 0.")
                    : ("Order total price Cannot be negative value."));
        }

        [Fact]
        public void DefaultValidation_OrderWithPastDateCreated_ShouldThrowException()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 7.80,
                OrderStatusId = 4,
                DateCreated = DateTime.Now.AddSeconds(-7)
            };

            Action action = () => _orderValidator.DefaultValidation(order);

            action.Should().Throw<ArgumentException>()
                .WithMessage("Order creation date is set to past value. " +
                             "Please input current date with 5 second precision.");
        }

        [Fact]
        public void DefaultValidation_OrderWithFutureDateCreated_ShouldThrowException()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 7.80,
                OrderStatusId = 4,
                DateCreated = DateTime.Now.AddSeconds(7)
            };

            Action action = () => _orderValidator.DefaultValidation(order);

            action.Should().Throw<ArgumentException>()
                .WithMessage("Order creation date is set to future value. " +
                             "Please input current date with 5 second precision.");
        }
    }
}