using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class ProductSqLiteRepository : IRepository<Product>
    {
        private UnitedMarketsDbContext _ctx;

        public ProductSqLiteRepository(UnitedMarketsDbContext context)
        {
            _ctx = context;
        }

        public FilteredList<Product> ReadAll(Filter filter)
        {
            var filteredList = new FilteredList<Product>() {FilterUsed = filter};


            filteredList.List = _ctx.Products
                .Where(p => p.MarketId == filter.MarketId)
                .Include(p => p.Origin)
                .Include(p => p.Market)
                .Include(p => p.AmountUnit)
                .ToList();
            filteredList.TotalCount = filteredList.List.Count();

            return filteredList;
        }


        public IEnumerable<Product> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public Product ReadById(long id)
        {
            throw new System.NotImplementedException();
        }

        public Product ReadByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public Product Create(Product entity)
        {
            throw new System.NotImplementedException();
        }

        public Product Update(Product entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}