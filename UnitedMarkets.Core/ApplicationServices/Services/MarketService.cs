using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

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

        public Market Create(Market entity)
        {
            throw new NotImplementedException();
        }

        public List<Market> GetAll()
        {
            return _marketRepository.ReadAll().ToList();
        }

        public FilteredList<Market> GetAll(Filter filter)
        {
            throw new NotImplementedException();
        }
    }
}