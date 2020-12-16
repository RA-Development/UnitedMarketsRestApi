using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class StatusSqLiteRepository : IRepository<OrderStatus>
    {
        private UnitedMarketsDbContext _ctx;

        public StatusSqLiteRepository(UnitedMarketsDbContext context)
        {
            _ctx = context;
        }

        public IEnumerable<OrderStatus> ReadAll()
        {
            return _ctx.OrderStatuses.ToList();
        }

        public OrderStatus ReadById(long id)
        {
            throw new System.NotImplementedException();
        }

        public OrderStatus ReadByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public OrderStatus Create(OrderStatus entity)
        {
            throw new System.NotImplementedException();
        }

        public OrderStatus Update(OrderStatus entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new System.NotImplementedException();
        }

        public FilteredList<OrderStatus> ReadAll(Filter filter)
        {
            throw new System.NotImplementedException();
        }
    }
}