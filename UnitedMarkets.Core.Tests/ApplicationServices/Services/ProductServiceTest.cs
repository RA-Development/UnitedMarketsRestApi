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
using UnitedMarkets.Core.PriceCalculator;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Services
{
    public class ProductServiceTest
    {
        private IValidator<Product> _productValidator;
        private IPriceCalculator _priceCalculator;
        private IValidator<Filter> _filterValidator;
        private Mock<IRepository<Product>> _repoMock;


        public ProductServiceTest()
        {
            _productValidator = new ProductValidator();
            _priceCalculator = new PriceCalculator.PriceCalculator();
            _filterValidator = new FilterValidator();
            _repoMock = new Mock<IRepository<Product>>();
        }

        [Fact]
        public void ProductService_IsOfTypeIProductService()
        {
            new ProductService(_repoMock.Object, _filterValidator, _priceCalculator, _productValidator)
                .Should().BeAssignableTo<IService<Product>>();
        }

        [Fact]
        public void NewProductService_WithNullRepository_ShouldThrowException()
        {
            Action action = () =>
                new ProductService(null as IRepository<Product>, _filterValidator, _priceCalculator, _productValidator);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage(("Repository Cannot be Null. (Parameter 'productRepository')"));
        }

        [Fact]
        public void NewProductService_WithNullFilterValidator_ShouldThrowException()
        {
            Action action = () =>
                new ProductService(_repoMock.Object, null as IValidator<Filter>, _priceCalculator, _productValidator);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage(("Validator Cannot be Null. (Parameter 'filterValidator')"));
        }


        [Fact]
        public void NewProductService_WithNullProductValidator_ShouldThrowException()
        {
            Action action = () =>
                new ProductService(_repoMock.Object, _filterValidator, _priceCalculator, null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage(("Validator Cannot be Null. (Parameter 'productValidator')"));
        }

        [Fact]
        public void NewProductService_WithNullPriceCalculator_ShouldThrowException()
        {
            Action action = () =>
                new ProductService(_repoMock.Object, _filterValidator, null as IPriceCalculator, _productValidator);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage(("Price Calculator Cannot be Null. (Parameter 'priceCalculator')"));
        }

        [Fact]
        public void GetAllProducts__ShouldCallRepoWithFilterInParams_Once()
        {
            IService<Product> productService =
                new ProductService(_repoMock.Object, _filterValidator, _priceCalculator, _productValidator);
            var filter = new Filter() {MarketId = 1};
            _repoMock.Setup(m
                => m.ReadAll(filter)).Returns(() => new FilteredList<Product>() {List = new List<Product>()});
            productService.GetAll(filter);
            _repoMock.Verify(repo => repo.ReadAll(filter), Times.Once);
        }
    }
}