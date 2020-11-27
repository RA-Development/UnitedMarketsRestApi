using System.Collections.Generic;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class MarketSqLiteRepository : IMarketRepository
    {
        public List<Market> ReadAll()
        {
            throw new System.NotImplementedException();
        }
    }
}