using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Infrastructure.Data.Repositories
{
    public class OrderSqLiteRepository : IRepository<Order>
    {
        private readonly UnitedMarketsDbContext _ctx;

        public OrderSqLiteRepository(UnitedMarketsDbContext ctx)
        {
            _ctx = ctx;
        }

        public Order Create(Order order)
        {
            var createdOrder = _ctx.Orders.Add(order);
            _ctx.SaveChanges();
            return createdOrder.Entity;
        }

        public IEnumerable<Order> ReadAll()
        {
            return _ctx.Orders.Include(o => o.Products);

            //    TODO: write the reason
            
            // return _ctx.Orders.Select(order => new Order
            // {
            //     Id = order.Id,
            //     DateCreated = order.DateCreated,
            //     TotalPrice = order.TotalPrice,
            //     Status = order.Status == null
            //         ? null
            //         : new Status
            //         {
            //             Id = order.Status.Id,
            //             Name = order.Status.Name
            //         }
            // }).ToList();
        }

        public FilteredList<Order> ReadAll(Filter filter)
        {
            throw new NotImplementedException();
        }

        public Order ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public Order ReadByName(string name)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order orderFromRequest)
        {
            _ctx.OrderLines.RemoveRange(_ctx.OrderLines.Where(ol => ol.OrderId == orderFromRequest.Id));
            _ctx.OrderLines.AddRange(orderFromRequest.Products);
            _ctx.Update(orderFromRequest);
            _ctx.Entry(orderFromRequest).Property(x => x.DateCreated).IsModified = false;
            _ctx.SaveChanges();

            var orderForClient = _ctx.Orders
                .Include(o => o.Products)
                .FirstOrDefault(o => o.Id == orderFromRequest.Id);

            if (orderForClient == null)
                throw new NullReferenceException("Order does not exist in database.");

            return orderForClient;
        }

        public Order Delete(int id)
        {
            try
            {
                var entry = _ctx.Orders.Remove(new Order {Id = id});
                _ctx.SaveChanges();
                return entry.Entity;
            }
            catch (Exception)
            {
                throw new DataException("The status could not be changed to \"Deleted\".");
            }
        }
    }
}