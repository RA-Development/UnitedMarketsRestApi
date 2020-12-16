using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Mock<IRepository<Order>> _repositoryMock;
        private readonly IValidator<Order> _orderValidator;
        private IService<Order> _orderService;
        private OrderLineValidator _olValidator;

        public OrderServiceTest()
        {
            _olValidator = new OrderLineValidator();
            _repositoryMock = new Mock<IRepository<Order>>();
            _orderValidator = new OrderValidator();
            _orderService = new OrderService(_repositoryMock.Object, _orderValidator, _olValidator);
        }

        [Fact]
        public void OrderService_ShouldBeOfTypeIServiceOrder()
        {
            new OrderService(_repositoryMock.Object, _orderValidator, _olValidator).Should()
                .BeAssignableTo<IService<Order>>();
        }

        [Fact]
        public void NewOrderService_WithNullRepository_ShouldThrowException()
        {
            Action action = () =>
                new OrderService(null, _orderValidator, _olValidator);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository cannot be null. (Parameter 'orderRepository')");
        }

        [Fact]
        public void NewService_WithNullOrderValidator_ShouldThrowException()
        {
            Action action = () => new OrderService(_repositoryMock.Object, null as OrderValidator, _olValidator);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Validator Cannot be Null. (Parameter 'orderValidator')");
        }

        [Fact]
        public void NewService_WithNullOrderLineValidator_ShouldThrowException()
        {
            Action action = () => new OrderService(_repositoryMock.Object, _orderValidator, null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Validator Cannot be Null. (Parameter 'orderLineValidator')");
        }

        [Fact]
        public void Update_ShouldCallRepositoryUpdateWithOrderInParams_Once()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 9};
            var orderLine2 = new OrderLine() {ProductId = 2, Quantity = 1, SubTotalPrice = 30};
            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 7",
                TotalPrice = 15.85,
                OrderStatusId = 4,
                DateCreated = DateTime.Parse("05/29/2015 05:50:06"),
                DateUpdated = DateTime.Now
            };

            _orderService.Update(order);
            _repositoryMock.Verify(r => r.Update(order));
        }


        [Fact]
        public void GetAll_ShouldCallOrderRepositoryReadAll_Once()
        {
            _orderService.GetAll();
            _repositoryMock.Verify(repository => repository.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetAll_ReturningNullFromRepository_ShouldThrowException()
        {
            //Arrange
            _repositoryMock.Setup(repo => repo.ReadAll()).Returns(() => null);

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

            _repositoryMock.Setup(repo => repo.ReadAll()).Returns(() => returnValue);


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
        public void CreateOrder_WithOrderInParams_ShouldCallRepoOnce()
        {
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine() {ProductId = 2, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 15.85,
                OrderStatusId = 1,
            };
            _orderService.Create(order);

            _repositoryMock.Verify(repo => repo.Create(order), Times.Once);
        }

        [Fact]
        public void CreateOrder_OrderWithValidParams_ShouldReturnOrder()
        {
            //    Arrange
            var orderLine1 = new OrderLine() {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70, OrderId = 2};
            var orderLine2 = new OrderLine() {ProductId = 3, Quantity = 74, SubTotalPrice = 654, OrderId = 2};

            var order = new Order()
            {
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                BillingAddress = "Esbjerg 8",
                TotalPrice = 111,
                OrderStatusId = 1,
            };

            var createdOrder = new Order()
            {
                Id = 2,
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                BillingAddress = "Esbjerg 8",
                TotalPrice = 74.30,
                OrderStatusId = 1,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            _repositoryMock.Setup(m => m.Create(order))
                .Returns(() => createdOrder);

            var expected = new Order()
            {
                Id = 2,
                Products = new List<OrderLine>() {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                BillingAddress = "Esbjerg 8",
                TotalPrice = 74.30,
                OrderStatusId = 1,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };
            //Act
            var actual = _orderService.Create(order);
            //Assert

            actual.Products.Should().BeEquivalentTo(expected.Products);
            actual.ShippingAddress.Should().BeEquivalentTo(expected.ShippingAddress);
            actual.BillingAddress.Should().BeEquivalentTo(expected.BillingAddress);
            actual.OrderStatus.Should().BeEquivalentTo(expected.OrderStatus);
            actual.TotalPrice.Should().Be(expected.TotalPrice);
            actual.DateCreated.Should().BeCloseTo(expected.DateCreated, 5.Seconds());
            actual.DateUpdated.Should().BeCloseTo(expected.DateUpdated, 5.Seconds());
            foreach (var item in expected.Products)
                actual.Id.Should().Be(item.OrderId);
        }
    }
}