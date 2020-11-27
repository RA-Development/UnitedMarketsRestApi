using System.Collections.Generic;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.DomainServices
{
    public interface IMarketRepository
    {
        List<Market> ReadAll();
    }
}
