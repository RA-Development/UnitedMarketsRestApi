using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class StatusSqLiteRepository : IRepository<Status>
    {
        private readonly UnitedMarketsDbContext _ctx;

        public StatusSqLiteRepository(UnitedMarketsDbContext context)
        {
            _ctx = context;
        }

        public Status Create(Status entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Status> ReadAll()
        {
            return _ctx.Status.ToList();
        }

        public FilteredList<Status> ReadAll(Filter filter)
        {
            throw new NotImplementedException();
        }

        public Status ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Status ReadByName(string name)
        {
            throw new NotImplementedException();
        }

        public Status Update(Status entity)
        {
            throw new NotImplementedException();
        }

        public Status Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}