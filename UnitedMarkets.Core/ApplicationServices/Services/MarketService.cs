using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class MarketService : IService<Market>
    {
        private readonly IRepository<Market> _marketRepository;

        public MarketService(IRepository<Market> marketRepository)
        {
            _marketRepository = marketRepository
                                ?? throw new ArgumentNullException(nameof(marketRepository),
                                    "Repository cannot be null.");
        }

        public List<Market> GetAll()
        {
            return _marketRepository.ReadAll().ToList();
        }
    }
}