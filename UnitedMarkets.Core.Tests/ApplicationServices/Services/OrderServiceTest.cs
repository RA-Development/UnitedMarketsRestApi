using System;
using System.Collections.Generic;
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
        private readonly Mock<IRepository<Order>> _orderRepoMock;
        private IValidator<Order> _orderValidator;
        private IService<Order> _orderService;

        public OrderServiceTest()
        {
            _orderRepoMock = new Mock<IRepository<Order>>();
            _orderValidator = new OrderValidator();
            _orderService = new OrderService(_orderRepoMock.Object, _orderValidator);
        }

        [Fact]
        public void OrderService_ShouldBeOfTypeIServiceOrder()
        {
            new OrderService(_orderRepoMock.Object, _orderValidator).Should().BeAssignableTo<IService<Order>>();
        }

        [Fact]
        public void NewOrderService_WithNullRepository_ShouldThrowException()
        {
            Action action = () =>
                new OrderService(null as IRepository<Order>, _orderValidator);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository cannot be null. (Parameter 'orderRepository')");
        }

        [Fact]
        public void GetAll_ShouldCallOrderRepositoryReadAll_Once()
        {
            _orderService.GetAll();
            _orderRepoMock.Verify(repository => repository.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetAll_ReturningNullFromRepository_ShouldThrowException()
        {
            //Arrange
            _orderRepoMock.Setup(repo => repo.ReadAll()).Returns(() => null);

            //Act
            Action action = () => _orderService.GetAll();

            //Assert TODO: Exception handling.
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAll_ShouldReturn_ExpectedListOfOrders()
        {
            //Arrange
            var returnValue = new List<Order>
            {
                new Order
                {
                    Id = 1, Products = new List<OrderLine>(), DateCreated = DateTime.Today.AddDays(-12),
                    TotalPrice = 100.45,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", OrderStatusId = 4
                },
                new Order
                {
                    Id = 2, Products = new List<OrderLine>(), DateCreated = DateTime.Today.AddDays(-1),
                    TotalPrice = 222.95,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", OrderStatusId = 4
                },
                new Order
                {
                    Id = 3, Products = new List<OrderLine>(), DateCreated = DateTime.Today, TotalPrice = 1255.95,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", OrderStatusId = 4
                }
            };

            _orderRepoMock.Setup(repo => repo.ReadAll()).Returns(() => returnValue);

            //Act
            var actual = _orderService.GetAll();

            //Assert
            var expected = new List<Order>
            {
                new Order
                {
                    Id = 1, Products = new List<OrderLine>(), DateCreated = DateTime.Today.AddDays(-12),
                    TotalPrice = 100.45,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", OrderStatusId = 4
                },
                new Order
                {
                    Id = 3, Products = new List<OrderLine>(), DateCreated = DateTime.Today, TotalPrice = 1255.95,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", OrderStatusId = 4
                },
                new Order
                {
                    Id = 2, Products = new List<OrderLine>(), DateCreated = DateTime.Today.AddDays(-1),
                    TotalPrice = 222.95,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", OrderStatusId = 4
                }
            };

            actual.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public void OrderService_IsOfTypeIOrderService()
        {
            new OrderService(_orderRepoMock.Object, _orderValidator).Should().BeAssignableTo<IService<Order>>();
        }

        [Fact]
        public void NewService_WithNullRepository_ShouldThrowException()
        {
            Action action = () => new OrderService(null as IRepository<Order>, _orderValidator);
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
            _orderService.Create(order);

            _orderRepoMock.Verify(repo => repo.Create(order), Times.Once);
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

            _orderRepoMock.Setup(m => m.Create(order))
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
            var actual = _orderService.Create(order);
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