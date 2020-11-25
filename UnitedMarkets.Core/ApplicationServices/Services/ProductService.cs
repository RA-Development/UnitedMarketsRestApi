using System;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository productRepo;

        public ProductService(IProductRepository productRepository)
        {
            productRepo = productRepository ??
                          throw new NullReferenceException("Product Repository Cannot be Null.");
        }

        public FilteredList<Product> GetAllProducts(Filter filter)
        {
            return productRepo.GetAllProducts(filter);
        }
    }
}