using System.Collections.Generic;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.DomainServices
{
    public interface IMarketRepository
    {
        Market Create(Market market);
        List<Market> ReadAll();
    }
}
