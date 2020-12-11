using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Validators
{
    public class OrderValidatorTest
    {
        private Mock<IOrderRepository> _orderRepoMock;
        private OrderService _orderService;
        private OrderValidator _orderValidator;


        public OrderValidatorTest()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _orderValidator = new OrderValidator();
            _orderService = new OrderService(_orderRepoMock.Object, _orderValidator);
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
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order has to contain billing address.");
        }

        [Fact]
        public void DefaultValidation_OrderWithEmptyShippingAddress_ShouldThrowException()
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
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order status has to be 'pending' on creation. (Pending status id = 4)");
        }

        [Fact]
        public void DefaultValidation_OrderWithZeroTotalPrice_ShouldThrowException()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 0,
                OrderStatusId = 4,
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order total price Cannot be 0.");
        }


        [Fact]
        public void DefaultValidation_OrderWithNegativeTotalPrice_ShouldThrowException()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = -7.80,
                OrderStatusId = 4,
            };
            Action action = () => _orderValidator.DefaultValidation(order);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Order total price Cannot be negative value.");
        }


        
    }
}