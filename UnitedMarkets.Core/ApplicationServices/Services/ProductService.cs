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
        private readonly IValidator<Filter> _filterValidator;
        private readonly IPriceCalculator _priceCalc;
        private readonly IRepository<Product> _productRepo;
        private readonly IValidator<Product> _productValidator;

        public ProductService(
            IRepository<Product> productRepository,
            IValidator<Filter> filterValidator,
            IPriceCalculator priceCalculator,
            IValidator<Product> productValidator)
        {
            _productRepo = productRepository ??
                           throw new ArgumentNullException(nameof(productRepository),
                               "Repository cannot be null.");
            _filterValidator = filterValidator ??
                               throw new ArgumentNullException(nameof(filterValidator),
                                   "Validator cannot be null.");
            _priceCalc = priceCalculator ??
                         throw new ArgumentNullException(nameof(priceCalculator),
                             "Price Calculator cannot be null.");
            _productValidator = productValidator ??
                                throw new ArgumentNullException(nameof(productValidator),
                                    "Validator cannot be null.");
        }

        public Product Create(Product entity)
        {
            throw new NotImplementedException();
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

        public List<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public Product Update(Product entity)
        {
            throw new NotImplementedException();
        }

        public Product Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}