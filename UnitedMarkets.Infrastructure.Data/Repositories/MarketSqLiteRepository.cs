using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class MarketSqLiteRepository : IMarketRepository
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

        public List<Market> ReadAll()
        {
            return _ctx.Markets.Select(market => new Market
            {
                Id = market.Id,
                Name = market.Name
            }).ToList();
        }
    }
}