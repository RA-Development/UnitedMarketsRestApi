using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Services
{
    public class OrderServiceTest
    {
        private readonly Mock<IRepository<Order>> _repositoryMock;

        public OrderServiceTest()
        {
            _repositoryMock = new Mock<IRepository<Order>>();
        }

        [Fact]
        public void OrderService_ShouldBeOfTypeIServiceOrder()
        {
            new OrderService(_repositoryMock.Object).Should().BeAssignableTo<IService<Order>>();
        }

        [Fact]
        public void NewOrderService_WithNullRepository_ShouldThrowException()
        {
            Action action = () =>
                new OrderService(null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository cannot be null. (Parameter 'orderRepository')");
        }

        [Fact]
        public void GetAll_ShouldCallOrderRepositoryReadAll_Once()
        {
            IService<Order> service = new OrderService(_repositoryMock.Object);
            service.GetAll();
            _repositoryMock.Verify(repository => repository.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetAll_ReturningNullFromRepository_ShouldThrowException()
        {
            //Arrange
            _repositoryMock.Setup(repo => repo.ReadAll()).Returns(() => null);
            var service = new OrderService(_repositoryMock.Object);

            //Act
            Action action = () => service.GetAll();

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
            var service = new OrderService(_repositoryMock.Object);

            //Act
            var actual = service.GetAll();

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
    }
}