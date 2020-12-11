using System;
using Microsoft.EntityFrameworkCore;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class OrderSqLiteRepository : IOrderRepository
    {
        private UnitedMarketsDbContext _ctx;

        public OrderSqLiteRepository(UnitedMarketsDbContext dbContext)
        {
            _ctx = dbContext;
        }

        public Order CreateOrder(Order order)
        {
            var createdOrder = _ctx.Orders.Add(order);
            _ctx.SaveChanges();
            return createdOrder.Entity;
        }
    }
}