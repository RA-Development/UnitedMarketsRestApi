using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class ProductSQLiteRepository : IProductRepository
    {
        private UnitedMarketsDBContext _ctx;

        public ProductSQLiteRepository(UnitedMarketsDBContext context)
        {
            _ctx = context;
        }

        public FilteredList<Product> GetAllProducts(Filter filter)
        {
            var filteredList = new FilteredList<Product>() {FilterUsed = filter};


            filteredList.List = _ctx.Products
                .Include(p => p.Origin)
                .ToList();
            filteredList.TotalCount = _ctx.Products.Count();

            return filteredList;
        }
    }
}