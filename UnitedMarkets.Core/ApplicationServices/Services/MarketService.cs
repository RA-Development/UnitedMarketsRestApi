using System;
using System.Collections.Generic;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class MarketService : IMarketService
    {
        private readonly IMarketRepository _marketRepository;

        public MarketService(IMarketRepository marketRepository)
        {
            this._marketRepository = marketRepository
                                     ?? throw new ArgumentNullException(nameof(marketRepository),"Repository cannot be null.");
        }

        public List<Market> GetAll()
        {    
            return _marketRepository.ReadAll();                
        }
    }
}