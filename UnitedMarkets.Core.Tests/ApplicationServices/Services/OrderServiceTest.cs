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
        private readonly Mock<IValidatorExtended<OrderLine>> _orderLineValidatorMock;
        private readonly IService<Order> _orderService;
        private readonly IService<Order> _orderServiceWithMockValidators;
        private readonly Mock<IValidatorExtended<Order>> _orderValidatorMock;
        private readonly Mock<IRepository<Order>> _repositoryMock;

        public OrderServiceTest()
        {
            _repositoryMock = new Mock<IRepository<Order>>();
            _orderValidatorMock = new Mock<IValidatorExtended<Order>>();
            _orderLineValidatorMock = new Mock<IValidatorExtended<OrderLine>>();
            _orderServiceWithMockValidators = new OrderService(_repositoryMock.Object, _orderValidatorMock.Object,
                _orderLineValidatorMock.Object);

            IValidatorExtended<Order> orderValidator = new OrderValidator();
            IValidatorExtended<OrderLine> orderLineValidator = new OrderLineValidator();
            _orderService = new OrderService(_repositoryMock.Object, orderValidator,
                orderLineValidator);
        }

        [Fact]
        public void OrderService_ShouldBeOfTypeIServiceOrder()
        {
            new OrderService(_repositoryMock.Object, _orderValidatorMock.Object, _orderLineValidatorMock.Object)
                .Should()
                .BeAssignableTo<IService<Order>>();
        }

        [Fact]
        public void NewOrderService_WithNullOrderRepository_ShouldThrowException()
        {
            Action action = () =>
                new OrderService(null, _orderValidatorMock.Object, _orderLineValidatorMock.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository cannot be null. (Parameter 'orderRepository')");
        }

        [Fact]
        public void NewOrderService_WithNullOrderValidator_ShouldThrowException()
        {
            Action action = () => new OrderService(_repositoryMock.Object, null, _orderLineValidatorMock.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Validator cannot be null. (Parameter 'orderValidator')");
        }

        [Fact]
        public void NewOrderService_WithNullOrderLineValidator_ShouldThrowException()
        {
            Action action = () => new OrderService(_repositoryMock.Object, _orderValidatorMock.Object, null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Validator cannot be null. (Parameter 'orderLineValidator')");
        }

        private static Order ArrangeCreate()
        {
            var orderLine1 = new OrderLine {OrderId = 1, ProductId = 1, Quantity = 2, SubTotalPrice = 100.00};
            var orderLine2 = new OrderLine {OrderId = 1, ProductId = 3, Quantity = 1, SubTotalPrice = 99.95};
            var order = new Order
            {
                Products = new List<OrderLine> {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 199.95
            };
            return order;
        }

        [Fact]
        public void Create_ShouldCallOrderRepositoryCreateWithOrderParam_Once()
        {
            var order = ArrangeCreate();
            _orderService.Create(order);
            _repositoryMock.Verify(repository => repository.Create(order), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallOrderLineValidatorDefaultValidationWithOrderLineParam_Once()
        {
            var order = ArrangeCreate();
            _orderServiceWithMockValidators.Create(order);
            _orderLineValidatorMock.Verify(validator => validator.DefaultValidation(order.Products.ElementAt(0)),
                Times.Once);
            _orderLineValidatorMock.Verify(validator => validator.DefaultValidation(order.Products.ElementAt(1)),
                Times.Once);
        }

        [Fact]
        public void Create_ShouldCallOrderValidatorDefaultValidationAndCreateValidationWithOrderParam_EachOnce()
        {
            var order = ArrangeCreate();
            _orderServiceWithMockValidators.Create(order);

            _orderValidatorMock.Verify(validator => validator.DefaultValidation(order), Times.Once);
            _orderValidatorMock.Verify(validator => validator.CreateValidation(order), Times.Once);
        }

        [Fact]
        public void Create_WithValidOrderParam_ShouldReturn_Order()
        {
            //    Arrange
            var order = ArrangeCreate();

            var orderLine1 = new OrderLine {ProductId = 1, Quantity = 2, SubTotalPrice = 100.00, OrderId = 2};
            var orderLine2 = new OrderLine {ProductId = 3, Quantity = 1, SubTotalPrice = 99.95, OrderId = 2};
            var createdOrder = new Order
            {
                Id = 2,
                Products = new List<OrderLine> {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 8",
                TotalPrice = 199.95,
                OrderStatusId = 1,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            _repositoryMock.Setup(repository => repository.Create(order))
                .Returns(createdOrder);

            var expected = createdOrder;

            //    Act
            var actual = _orderService.Create(order);

            //    Assert
            actual.Id.Should().Be(expected.Id);
            actual.Products.Should().BeEquivalentTo(expected.Products);
            actual.BillingAddress.Should().BeEquivalentTo(expected.BillingAddress);
            actual.ShippingAddress.Should().BeEquivalentTo(expected.ShippingAddress);
            actual.TotalPrice.Should().Be(expected.TotalPrice);
            actual.OrderStatus.Should().BeEquivalentTo(expected.OrderStatus);
            actual.DateCreated.Should().BeCloseTo(expected.DateCreated, 5.Seconds());
            actual.DateUpdated.Should().BeCloseTo(expected.DateUpdated, 5.Seconds());
            foreach (var item in expected.Products)
                actual.Id.Should().Be(item.OrderId);
        }

        [Fact]
        public void GetAll_ShouldCallOrderRepositoryReadAll_Once()
        {
            _orderService.GetAll();
            _repositoryMock.Verify(repository => repository.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetAll_NullReturnFromOrderRepository_ShouldThrowException()
        {
            //Arrange
            _repositoryMock.Setup(repository => repository.ReadAll()).Returns(() => null);

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
                    TotalPrice = 100.45, BillingAddress = "Billing Street", ShippingAddress = "Shipping Street",
                    OrderStatusId = 4
                },
                new Order
                {
                    Id = 2, Products = new List<OrderLine>(), DateCreated = DateTime.Today.AddDays(-1),
                    TotalPrice = 222.95, BillingAddress = "Billing Street", ShippingAddress = "Shipping Street",
                    OrderStatusId = 4
                },
                new Order
                {
                    Id = 3, Products = new List<OrderLine>(), DateCreated = DateTime.Today, TotalPrice = 1255.95,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", OrderStatusId = 4
                }
            };

            _repositoryMock.Setup(repository => repository.ReadAll()).Returns(() => returnValue);


            //Act
            var actual = _orderService.GetAll();

            //Assert
            var expected = new List<Order>
            {
                new Order
                {
                    Id = 1, Products = new List<OrderLine>(), DateCreated = DateTime.Today.AddDays(-12),
                    TotalPrice = 100.45, BillingAddress = "Billing Street", ShippingAddress = "Shipping Street",
                    OrderStatusId = 4
                },
                new Order
                {
                    Id = 3, Products = new List<OrderLine>(), DateCreated = DateTime.Today, TotalPrice = 1255.95,
                    BillingAddress = "Billing Street", ShippingAddress = "Shipping Street", OrderStatusId = 4
                },
                new Order
                {
                    Id = 2, Products = new List<OrderLine>(), DateCreated = DateTime.Today.AddDays(-1),
                    TotalPrice = 222.95, BillingAddress = "Billing Street", ShippingAddress = "Shipping Street",
                    OrderStatusId = 4
                }
            };

            actual.Should().BeEquivalentTo(expected);
        }

        private static Order ArrangeUpdate()
        {
            var orderLine1 = new OrderLine {OrderId = 1, ProductId = 1, Quantity = 3, SubTotalPrice = 9};
            var orderLine2 = new OrderLine {OrderId = 1, ProductId = 2, Quantity = 1, SubTotalPrice = 30};
            var order = new Order
            {
                Id = 1,
                Products = new List<OrderLine> {orderLine1, orderLine2},
                BillingAddress = "Esbjerg 7",
                ShippingAddress = "Esbjerg 7",
                TotalPrice = 39,
                OrderStatusId = 4,
                DateCreated = DateTime.Parse("05/29/2015 05:50:06"),
                DateUpdated = DateTime.Now
            };
            return order;
        }

        [Fact]
        public void Update_ShouldCallOrderRepositoryUpdateWithOrderInParam_Once()
        {
            var order = ArrangeUpdate();
            _orderService.Update(order);
            _repositoryMock.Verify(repository => repository.Update(order));
        }

        [Fact]
        public void
            Update_ShouldCallOrderValidatorIdValidationDefaultValidationUpdateValidationWithOrderInParam_EachOnce()
        {
            var order = ArrangeUpdate();
            _orderServiceWithMockValidators.Update(order);
            _orderValidatorMock.Verify(validator => validator.IdValidation(order.Id), Times.Once);
            _orderValidatorMock.Verify(validator => validator.DefaultValidation(order), Times.Once);
            _orderValidatorMock.Verify(validator => validator.UpdateValidation(order), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallOrderLineValidatorDefaultValidationWithOrderLineParam_Once()
        {
            var order = ArrangeUpdate();
            _orderServiceWithMockValidators.Update(order);
            _orderLineValidatorMock.Verify(validator => validator.DefaultValidation(order.Products.ElementAt(0)),
                Times.Once);
            _orderLineValidatorMock.Verify(validator => validator.DefaultValidation(order.Products.ElementAt(1)),
                Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallOrderRepositoryDeleteWithIdParam_Once()
        {
            const int orderId = 1;
            _orderServiceWithMockValidators.Delete(orderId);
            _repositoryMock.Verify(repository => repository.Delete(orderId), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallOrderValidatorIdValidationWithIdParam_Once()
        {
            const int orderId = 1;
            _orderServiceWithMockValidators.Delete(orderId);
            _orderValidatorMock.Verify(validator => validator.IdValidation(orderId), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallOrderValidatorDeleteValidationWithOrderParam_Once()
        {
            const int orderId = 1;
            var returnedOrder = _orderServiceWithMockValidators.Delete(orderId);
            _orderValidatorMock.Verify(validator => validator.DeleteValidation(returnedOrder), Times.Once);
        }

        [Fact]
        public void Delete_WithValidIdParam_ShouldReturn_DeletedOrderWithIsDeletedAsTrue()
        {
            const int orderId = 1;
            var returnValue = new Order
            {
                Id = 1,
                IsDeleted = true
            };
            _repositoryMock.Setup(repository => repository.Delete(orderId)).Returns(returnValue);
            var actual = _orderService.Delete(orderId);
            var expected = returnValue;
            actual.Id.Should().Be(expected.Id);
            actual.IsDeleted.Should().Be(expected.IsDeleted);
        }
    }
}