using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Extensions;
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
        private readonly IService<Order> _orderService;
        private readonly Mock<IRepository<Order>> _repositoryMock;
        private readonly Mock<IValidatorExtended<Order>> _validatorMock;

        public OrderServiceTest()
        {
            _repositoryMock = new Mock<IRepository<Order>>();
            _validatorMock = new Mock<IValidatorExtended<Order>>();
            _orderService = new OrderService(_repositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public void OrderService_ShouldBeOfTypeIServiceOrder()
        {
            new OrderService(_repositoryMock.Object, _validatorMock.Object).Should().BeAssignableTo<IService<Order>>();
        }

        [Fact]
        public void NewOrderService_WithNullRepository_ShouldThrowException()
        {
            Action action = () =>
                new OrderService(null, _validatorMock.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository cannot be null. (Parameter 'orderRepository')");
        }

        [Fact]
        public void NewOrderService_WithNullValidator_ShouldThrowException()
        {
            Action action = () => new OrderService(_repositoryMock.Object, null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Validator Cannot be Null. (Parameter 'orderValidator')");
        }


        [Fact]
        public void GetAll_ShouldCallOrderRepositoryReadAll_Once()
        {
            _orderService.GetAll();
            _repositoryMock.Verify(repository => repository.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetAll_NullReturnFromRepository_ShouldThrowException()
        {
            //Arrange
            _repositoryMock.Setup(repo => repo.ReadAll()).Returns(() => null);

            //Act
            Action action = () => _orderService.GetAll();

            //Assert TODO: Exception handling.
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAll_ShouldReturn_ExpectedOrders()
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

            var expected = returnValue;

            //Act
            var actual = _orderService.GetAll();

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_ShouldCallOrderRepositoryCreateWithOrderParam_Once()
        {
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order
            {
                Products = new List<OrderLine> {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 15.85,
                OrderStatusId = 1
            };
            _orderService.Create(order);

            _repositoryMock.Verify(repo => repo.Create(order), Times.Once);
        }

        //TODO: Create_ShouldCallOrderValidatorDefaultValidationWithOrderParam_Once()

        [Fact]
        public void Create_WithValidOrderParam_ShouldReturn_Order()
        {
            //    Arrange
            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};
            var orderLine2 = new OrderLine {ProductId = 1, Quantity = 3, SubTotalPrice = 874.70};

            var order = new Order
            {
                Products = new List<OrderLine> {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                BillingAddress = "Esbjerg 8",
                TotalPrice = 74.30,
                OrderStatusId = 1
            };

            var createdOrder = new Order
            {
                Products = new List<OrderLine> {orderLine1, orderLine2},
                ShippingAddress = "Esbjerg 8",
                BillingAddress = "Esbjerg 8",
                TotalPrice = 74.30,
                OrderStatusId = 1,
                DateCreated = DateTime.Now
            };

            _repositoryMock.Setup(m => m.Create(order))
                .Returns(() => createdOrder);

            var expected = createdOrder;

            //    Act
            var actual = _orderService.Create(order);

            //    Assert
            actual.Products.Should().BeEquivalentTo(expected.Products);
            actual.ShippingAddress.Should().BeEquivalentTo(expected.ShippingAddress);
            actual.BillingAddress.Should().BeEquivalentTo(expected.BillingAddress);
            actual.OrderStatus.Should().BeEquivalentTo(expected.OrderStatus);
            actual.TotalPrice.Should().Be(expected.TotalPrice);
            actual.DateCreated.Should().BeCloseTo(expected.DateCreated, 10.Seconds());
        }

        [Fact]
        public void Delete_ShouldCallOrderRepositoryDeleteWithIdParam_Once()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<OrderLine>(),
                DateCreated = DateTime.Now,
                TotalPrice = 199.95,
                BillingAddress = "John Doe, Billing Street 12, 6700 Esbjerg",
                ShippingAddress = "Jane Doe, Shipping Street 33, 6700 Esbjerg",
                OrderStatusId = 1,
                OrderStatus = new OrderStatus {Id = 1, Name = "Pending"}
            };
            _orderService.Delete(order.Id);
            _repositoryMock.Verify(repository => repository.Delete(order.Id), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallOrderValidatorIdValidationWithIdParam_Once()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<OrderLine>(),
                DateCreated = DateTime.Now,
                TotalPrice = 199.95,
                BillingAddress = "John Doe, Bill Street 12, 6700 Esbjerg",
                ShippingAddress = "Jane Doe, Ship Street 33, 6700 Esbjerg",
                OrderStatusId = 1,
                OrderStatus = new OrderStatus {Id = 1, Name = "Pending"}
            };
            _orderService.Delete(order.Id);
            _validatorMock.Verify(validator => validator.IdValidation(order.Id), Times.Once);
        }
        
        [Fact]
        public void Delete_ShouldCallOrderValidatorIsDeletedValidationWithOrderParam_Once()
        {
            var order = new Order
            {
                Id = 1,
                IsDeleted = true
            };
            var returnedOrder = _orderService.Delete(order.Id);
            _validatorMock.Verify(validator => validator.IsDeletedValidation(returnedOrder), Times.Once);
        }

        [Fact]
        public void Delete_WithValidIdParam_ShouldReturn_DeletedOrderWithIsDeletedAsTrue()
        {
            var order = new Order
            {
                Id = 1,
                Products = new List<OrderLine>(),
                DateCreated = DateTime.Now,
                TotalPrice = 199.95,
                BillingAddress = "John Doe, Bill Street 12, 6700 Esbjerg",
                ShippingAddress = "Jane Doe, Ship Street 33, 6700 Esbjerg",
                OrderStatusId = 1,
                OrderStatus = new OrderStatus {Id = 1, Name = "Pending"}
            };
            var returnValue = new Order
            {
                Id = 1,
                IsDeleted = true
            };
            _repositoryMock.Setup(repository => repository.Delete(order.Id)).Returns(returnValue);
            var actual = _orderService.Delete(order.Id);
            var expected = returnValue;
            actual.Id.Should().Be(expected.Id);
            actual.IsDeleted.Should().Be(expected.IsDeleted);
        }
    }
}