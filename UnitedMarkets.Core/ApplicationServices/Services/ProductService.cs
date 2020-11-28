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
        private IFilterValidator _filterValidator;
        private IPriceCalculator _priceCalc;

        public ProductService(
            IProductRepository productRepository,
            IFilterValidator filterValidator,
            IPriceCalculator priceCalculator)
        {
            _productRepo = productRepository ??
                           throw new NullReferenceException("Product Repository Cannot be Null.");
            _filterValidator = filterValidator ??
                               throw new NullReferenceException("Filter Validator Cannot be Null.");
            _priceCalc = priceCalculator ??
                         throw new NullReferenceException("Price Calculator Cannot be Null.");
        }

        public FilteredList<Product> GetAllProducts(Filter filter)
        {
            _filterValidator.DefaultValidation(filter);
            var filteredList = _productRepo.GetAllProducts(filter);
            foreach (var product in filteredList.List)
            {
                _priceCalc.CalculatePrice(product);
            }

            return filteredList;
        }
    }
}