using System;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepo;
        private IFilterValidator _filterValidator;

        public ProductService(
            IProductRepository productRepository,
            IFilterValidator filterValidator)
        {
            _productRepo = productRepository ??
                           throw new NullReferenceException("Product Repository Cannot be Null.");
            _filterValidator = filterValidator ??
                               throw new NullReferenceException("Filter Validator Cannot be Null.");
        }

        public FilteredList<Product> GetAllProducts(Filter filter)
        {
            _filterValidator.DefaultValidation(filter);
            return _productRepo.GetAllProducts(filter);
        }
    }
}