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
    public class MarketServiceTest
    {
        private readonly Mock<IRepository<Market>> _repositoryMock;

        public MarketServiceTest()
        {
            _repositoryMock = new Mock<IRepository<Market>>();
        }

        [Fact]
        public void MarketService_ShouldBeOfTypeIMarketService()
        {
            new MarketService(_repositoryMock.Object).Should().BeAssignableTo<IService<Market>>();
        }

        [Fact]
        public void NewMarketService_WithNullRepository_ShouldThrowException()
        {
            Action action = () => new MarketService(null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository cannot be null. (Parameter 'marketRepository')");
        }

        [Fact]
        public void GetAll_ShouldCallMarketRepositoryReadAll_Once()
        {
            IService<Market> marketService = new MarketService(_repositoryMock.Object);
            marketService.GetAll();
            _repositoryMock.Verify(repo => repo.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetAll_ShouldReturn_ExpectedListOfMarkets()
        {
            //Arrange
            var returnValue = new List<Market>
            {
                new Market {Id = 1, Name = "Bilka"},
                new Market {Id = 2, Name = "Netto"},
                new Market {Id = 3, Name = "Kvickly"}
            };

            _repositoryMock.Setup(repo => repo.ReadAll()).Returns(() => returnValue);
            var service = new MarketService(_repositoryMock.Object);

            //Act
            var actual = service.GetAll();

            //Assert
            var expected = new List<Market>
            {
                new Market {Id = 1, Name = "Bilka"},
                new Market {Id = 3, Name = "Kvickly"},
                new Market {Id = 2, Name = "Netto"}
            };

            actual.Should().BeEquivalentTo(expected);
        }
    }
}