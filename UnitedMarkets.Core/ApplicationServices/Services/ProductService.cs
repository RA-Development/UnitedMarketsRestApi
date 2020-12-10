using System;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;
using UnitedMarkets.Core.PriceCalculator;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepo;
        private IValidator<Filter> _filterValidator;
        private IPriceCalculator _priceCalc;
        private IValidator<Product> _productValidator;

        public ProductService(
            IProductRepository productRepository,
            IValidator<Filter> filterValidator,
            IPriceCalculator priceCalculator,
            IValidator<Product> productValidator)
        {
            _productRepo = productRepository ??
                           throw new NullReferenceException("Product Repository Cannot be Null.");
            _filterValidator = filterValidator ??
                               throw new NullReferenceException("Filter Validator Cannot be Null.");
            _priceCalc = priceCalculator ??
                         throw new NullReferenceException("Price Calculator Cannot be Null.");
            _productValidator = productValidator ??
                                throw new NullReferenceException("Product Validator Cannot be Null.");
        }

        public FilteredList<Product> GetAllProducts(Filter filter)
        {
            _filterValidator.DefaultValidation(filter);
            var filteredList = _productRepo.GetAllProducts(filter);
            foreach (var product in filteredList.List)
            {
                _productValidator.DefaultValidation(product);
                _priceCalc.CalculatePrice(product);
            }

            return filteredList;
        }
    }
}