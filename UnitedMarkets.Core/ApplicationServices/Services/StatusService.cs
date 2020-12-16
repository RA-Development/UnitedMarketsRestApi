using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class StatusService : IService<OrderStatus>
    {
        private IRepository<OrderStatus> _statusRepo;

        public StatusService(IRepository<OrderStatus> statusRepository)
        {
            _statusRepo = statusRepository ??
                          throw new ArgumentNullException(nameof(statusRepository),
                              "Repository Cannot be Null.");
        }

        public OrderStatus Create(OrderStatus entity)
        {
            throw new System.NotImplementedException();
        }

        public List<OrderStatus> GetAll()
        {
            return _statusRepo.ReadAll().ToList();
        }

        public FilteredList<OrderStatus> GetAll(Filter filter)
        {
            throw new System.NotImplementedException();
        }

        public OrderStatus Update(OrderStatus entity)
        {
            throw new System.NotImplementedException();
        }
    }
}