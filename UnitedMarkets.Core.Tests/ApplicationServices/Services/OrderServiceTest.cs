using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Services
{
    public class OrderServiceTest
    {
        private Mock<IOrderRepository> _orderRepoMock;
        private IOrderValidator _orderValidator;
        private IOrderService _orderService;

        public OrderServiceTest()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _orderValidator = new OrderValidator();
            _orderService = new OrderService(_orderRepoMock.Object, _orderValidator);
        }

        [Fact]
        public void OrderService_IsOfTypeIOrderService()
        {
            new OrderService(_orderRepoMock.Object, _orderValidator).Should().BeAssignableTo<IOrderService>();
        }

        [Fact]
        public void NewService_WithNullRepository_ShouldThrowException()
        {
            Action action = () => new OrderService(null as IOrderRepository, _orderValidator);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository Cannot be Null. (Parameter 'orderRepository')");
        }

        [Fact]
        public void NewService_WithNullValidator_ShouldThrowException()
        {
            Action action = () => new OrderService(_orderRepoMock.Object, null as OrderValidator);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Validator Cannot be Null. (Parameter 'orderValidator')");
        }


        [Fact]
        public void CreateOrder_WithOrderInParams_ShouldCallRepoOnce()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 15.85,
                OrderStatusId = 4,
            };
            _orderService.CreateOrder(order);

            _orderRepoMock.Verify(repo => repo.CreateOrder(order), Times.Once);
        }


        [Fact]
        public void CreateOrder_OrderWithValidParams_ShouldReturnOrder()
        {
            //    Arrange
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                BillingAddress = "Esbjerg 8",
                TotalPrice = 74.30,
                OrderStatusId = 4,
            };

            var createdOrder = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                BillingAddress = "Esbjerg 8",
                TotalPrice = 74.30,
                OrderStatusId = 4,
                DateCreated = DateTime.Now
            };

            _orderRepoMock.Setup(m => m.CreateOrder(order))
                .Returns(() => createdOrder);

            var expected = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                BillingAddress = "Esbjerg 8",
                TotalPrice = 74.30,
                OrderStatusId = 4,
                DateCreated = DateTime.Now
            };
            //Act
            var actual = _orderService.CreateOrder(order);
            //Assert

            actual.Products.Should().BeEquivalentTo(expected.Products);
            actual.ShippingAddress.Should().BeEquivalentTo(expected.ShippingAddress);
            actual.BillingAddress.Should().BeEquivalentTo(expected.BillingAddress);
            actual.OrderStatus.Should().BeEquivalentTo(expected.OrderStatus);
            actual.TotalPrice.Should().Be(expected.TotalPrice);
            actual.DateCreated.Should().BeCloseTo(expected.DateCreated, 10.Seconds());
        }
    }
}