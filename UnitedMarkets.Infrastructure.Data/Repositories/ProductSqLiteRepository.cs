using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class ProductSqLiteRepository : IRepository<Product>
    {
        private readonly UnitedMarketsDbContext _ctx;

        public ProductSqLiteRepository(UnitedMarketsDbContext context)
        {
            _ctx = context;
        }

        public Product Create(Product entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> ReadAll()
        {
            throw new NotImplementedException();
        }

        public FilteredList<Product> ReadAll(Filter filter)
        {
            var filteredList = new FilteredList<Product> {FilterUsed = filter};

            filteredList.List = _ctx.Products
                .Where(p => p.MarketId == filter.MarketId)
                .Include(p => p.Origin)
                .Include(p => p.Market)
                .Include(p => p.AmountUnit)
                .ToList();
            filteredList.TotalCount = filteredList.List.Count();

            return filteredList;
        }

        public Product ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Product ReadByName(string name)
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