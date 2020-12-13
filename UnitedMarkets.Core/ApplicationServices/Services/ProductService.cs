using System;
using System.Collections.Generic;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;
using UnitedMarkets.Core.PriceCalculator;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class ProductService : IService<Product>
    {
        private IRepository<Product> _productRepo;
        private IValidator<Filter> _filterValidator;
        private IPriceCalculator _priceCalc;
        private IValidator<Product> _productValidator;

        public ProductService(
            IRepository<Product> productRepository,
            IValidator<Filter> filterValidator,
            IPriceCalculator priceCalculator,
            IValidator<Product> productValidator)
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

        public FilteredList<Product> GetAll(Filter filter)
        {
            _filterValidator.DefaultValidation(filter);
            var filteredList = _productRepo.ReadAll(filter);
            foreach (var product in filteredList.List)
            {
                _productValidator.DefaultValidation(product);
                _priceCalc.CalculatePrice(product);
            }

            return filteredList;
        }

        public Product Create(Product entity)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}