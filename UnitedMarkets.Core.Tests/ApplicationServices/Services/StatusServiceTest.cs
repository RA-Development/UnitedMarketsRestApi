using System;
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
        private readonly Mock<IRepository<Status>> _repoMock;
        private readonly StatusService _service;

        public StatusServiceTest()
        {
            _repoMock = new Mock<IRepository<Status>>();
            _service = new StatusService(_repoMock.Object);
        }

        [Fact]
        public void Service_ShouldBeOfTypeIStatusService()
        {
            new StatusService(_repoMock.Object).Should().BeAssignableTo<IService<Status>>();
        }

        [Fact]
        public void NewService_WithNullRepository_ShouldThrowException()
        {
            Action action = () =>
                new StatusService(null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository cannot be null. (Parameter 'statusRepository')");
        }

        [Fact]
        public void GetAllStatuses__ShouldCallRepoReadAll_Once()
        {
            _service.GetAll();
            _repoMock.Verify(repo => repo.ReadAll(), Times.Once);
        }
    }
}