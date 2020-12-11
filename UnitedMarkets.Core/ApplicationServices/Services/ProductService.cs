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
        private IProductValidator _productValidator;

        public ProductService(
            IProductRepository productRepository,
            IFilterValidator filterValidator,
            IPriceCalculator priceCalculator,
            IProductValidator productValidator)
        {
            _productRepo = productRepository ??
                           throw new ArgumentNullException(nameof(productRepository),
                               "Repository Cannot be Null.");
            _filterValidator = filterValidator ??
                               throw new ArgumentNullException(nameof(filterValidator),
                                   "Validator Cannot be Null.");
            _priceCalc = priceCalculator ??
                         throw new ArgumentNullException(nameof(priceCalculator),
                             "Price Calculator Cannot be Null.");
            _productValidator = productValidator ??
                                throw new ArgumentNullException(nameof(productValidator),
                                    "Validator Cannot be Null.");
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