using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class MarketSqLiteRepository : IRepository<Market>
    {
        private readonly UnitedMarketsDbContext _ctx;

        public MarketSqLiteRepository(UnitedMarketsDbContext ctx)
        {
            _ctx = ctx;
        }

        public Market Create(Market market)
        {
            var entry = _ctx.Markets.Add(new Market
            {
                Id = market.Id,
                Name = market.Name
            });
            _ctx.SaveChanges();
            return new Market
            {
                Id = entry.Entity.Id,
                Name = entry.Entity.Name
            };
        }

        public IEnumerable<Market> ReadAll()
        {
            return _ctx.Markets.Select(market => new Market
            {
                Id = market.Id,
                Name = market.Name
            }).ToList();
        }

        public FilteredList<Market> ReadAll(Filter filter)
        {
            throw new NotImplementedException();
        }

        public Market ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Market ReadByName(string name)
        {
            throw new NotImplementedException();
        }

        public Market Update(Market entity)
        {
            throw new NotImplementedException();
        }

        public Market Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}