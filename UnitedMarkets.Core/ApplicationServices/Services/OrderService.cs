using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class OrderService : IService<Order>
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository ??
                               throw new ArgumentNullException(nameof(orderRepository), "Repository cannot be null.");
        }

        public List<Order> GetAll()
        {
            return _orderRepository.ReadAll().ToList();
        }
    }
}