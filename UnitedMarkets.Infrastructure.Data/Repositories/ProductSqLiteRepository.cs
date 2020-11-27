using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class ProductSqLiteRepository : IProductRepository
    {
        private UnitedMarketsDbContext _ctx;

        public ProductSqLiteRepository(UnitedMarketsDbContext context)
        {
            _ctx = context;
        }

        public FilteredList<Product> GetAllProducts(Filter filter)
        {
            var filteredList = new FilteredList<Product>() {FilterUsed = filter};


            filteredList.List = _ctx.Products
                .Where(p => p.MarketId == filter.MarketId)
                .Include(p => p.OriginCountry)
                .Include(p => p.Market)
                .Include(p => p.AmountUnit)
                .ToList();
            filteredList.TotalCount = _ctx.Products.Count();

            return filteredList;
        }
    }
}