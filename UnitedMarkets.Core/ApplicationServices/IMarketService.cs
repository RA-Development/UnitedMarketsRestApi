using System.Collections.Generic;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IMarketService
    {
        List<Market> GetAll();
    }
}