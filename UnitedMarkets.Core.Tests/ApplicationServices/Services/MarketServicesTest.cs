using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Services
{
    public class MarketServicesTest
    {
        [Fact]
        public void NewService_WithNullRepository_ShouldThrowException()
        {
            Action action = () => new MarketService(null as IMarketRepository);
            action.Should().Throw<ArgumentNullException>("Repository cannot be null");
        }

        [Fact]
        public void MarketService_ShouldBeOfTypeIMarketService()
        {
            var repositoryMock = new Mock<IMarketRepository>();
            new MarketService(repositoryMock.Object).Should().BeAssignableTo<IMarketService>();
        }

        [Fact]
        public void Get_ShouldCallMarketRepositoryGet_Once()
        {
            var marketRepositoryMock = new Mock<IMarketRepository>();
            IMarketService marketService = new MarketService(marketRepositoryMock.Object);
            marketService.GetAll();
            marketRepositoryMock.Verify(repo => repo.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturn_ListOfMarkets()
        {
            //Arrange
            var repositoryMock = new Mock<IMarketRepository>();
            var returnValue = new List<Market>
            {
                new Market {Id = 1, Name = "Bilka"},
                new Market { Id = 2, Name = "Netto" }
            };

            repositoryMock.Setup(repo => repo.ReadAll()).Returns(() => returnValue);
            var service = new MarketService(repositoryMock.Object);

            //Act
            var list = service.GetAll();

            //Assert
            list.Should().BeOfType<List<Market>>("the return type has to be List<Market>");
        }
    }
}