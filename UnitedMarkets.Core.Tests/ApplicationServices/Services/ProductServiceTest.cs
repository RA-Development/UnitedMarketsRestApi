using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Filtering;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Services
{
    public class ProductServiceTest
    {
        [Fact]
        public void ProductService_IsOfTypeIProductService()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            new ProductService(productRepositoryMock.Object).Should().BeAssignableTo<IProductService>();
        }

        [Fact]
        public void NewProductService_WithNullRepository_ShouldThrowException()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            Action action = () => new ProductService(null as IProductRepository);
            action.Should().Throw<NullReferenceException>().WithMessage(("Product Repository Cannot be Null."));
        }

        [Fact]
        public void GetAllProducts__ShouldCallRepoWithFilterInParams_Once()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            IProductService productService = new ProductService(productRepositoryMock.Object);
            Filter filter = new Filter();
            productService.GetAllProducts(filter);
            productRepositoryMock.Verify(repo => repo.GetAllProducts(filter), Times.Once);
        }
    }
}