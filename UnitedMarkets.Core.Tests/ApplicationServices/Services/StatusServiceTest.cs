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
    public class StatusServiceTest
    {
        private Mock<IRepository<OrderStatus>> _repoMock;
        private StatusService _service;

        public StatusServiceTest()
        {
            _repoMock = new Mock<IRepository<OrderStatus>>();
            _service = new StatusService(_repoMock.Object);
        }

        [Fact]
        public void Service_ShouldBeOfTypeIStatusService()
        {
            new StatusService(_repoMock.Object).Should().BeAssignableTo<IService<OrderStatus>>();
        }

        [Fact]
        public void NewService_WithNullRepository_ShouldThrowException()
        {
            Action action = () =>
                new StatusService(null as IRepository<OrderStatus>);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage(("Repository Cannot be Null. (Parameter 'statusRepository')"));
        }

        [Fact]
        public void GetAllStatuses__ShouldCallRepoReadAll_Once()
        {
            _service.GetAll();
            _repoMock.Verify(repo => repo.ReadAll(), Times.Once);
        }
    }
}