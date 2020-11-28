using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Services
{
    public class ProductServiceTest
    {
        [Fact]
        public void ProductService_IsOfTypeIProductService()
        {
            var calc = new PriceCalculator.PriceCalculator();
            var productRepositoryMock = new Mock<IProductRepository>();
            var filterValidator = new FilterValidator();
            new ProductService(productRepositoryMock.Object, filterValidator, calc)
                .Should().BeAssignableTo<IProductService>();
        }

        [Fact]
        public void NewProductService_WithNullRepository_ShouldThrowException()
        {
            var calc = new PriceCalculator.PriceCalculator();
            var productRepositoryMock = new Mock<IProductRepository>();
            var filterValidator = new FilterValidator();
            Action action = () => new ProductService(null as IProductRepository, filterValidator, calc);
            action.Should().Throw<NullReferenceException>()
                .WithMessage(("Product Repository Cannot be Null."));
        }

        [Fact]
        public void NewProductService_WithNullFilterValidator_ShouldThrowException()
        {
            var calc = new PriceCalculator.PriceCalculator();
            var productRepositoryMock = new Mock<IProductRepository>();
            Action action = () => new ProductService(productRepositoryMock.Object, null as IFilterValidator, calc);
            action.Should().Throw<NullReferenceException>()
                .WithMessage(("Filter Validator Cannot be Null."));
        }


        [Fact]
        public void GetAllProducts__ShouldCallRepoWithFilterInParams_Once()
        {
            var calc = new PriceCalculator.PriceCalculator();
            var productRepositoryMock = new Mock<IProductRepository>();
            var filterValidator = new FilterValidator();
            IProductService productService = new ProductService(productRepositoryMock.Object, filterValidator, calc);
            var filter = new Filter() {MarketId = 1};
            productRepositoryMock.Setup(m
                => m.GetAllProducts(filter)).Returns(() => new FilteredList<Product>() {List = new List<Product>()});
            productService.GetAllProducts(filter);
            productRepositoryMock.Verify(repo => repo.GetAllProducts(filter), Times.Once);
        }
    }
}