using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class OrderSqLiteRepository : IRepository<Order>
    {
        private readonly UnitedMarketsDbContext _ctx;

        public OrderSqLiteRepository(UnitedMarketsDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Order> ReadAll()
        {
            return _ctx.Orders.Select(order => new Order
            {
                Id = order.Id,
                DateCreated = order.DateCreated,
                TotalPrice = order.TotalPrice,
                OrderStatus = order.OrderStatus == null
                    ? null
                    : new OrderStatus
                    {
                        Id = order.OrderStatus.Id,
                        Name = order.OrderStatus.Name
                    }
            }).ToList();
        }

        public Order ReadById(long id)
        {
            throw new NotImplementedException();
        }

        public Order ReadByName(string name)
        {
            throw new NotImplementedException();
        }

        public Order Create(Order entity)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}