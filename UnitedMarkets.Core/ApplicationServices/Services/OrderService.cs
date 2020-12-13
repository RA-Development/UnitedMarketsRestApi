using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class OrderService : IService<Order>
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IValidator<Order> _orderValidator;

        public OrderService(IRepository<Order> orderRepository, IValidator<Order> orderValidator)
        {
            _orderRepo = orderRepository ??
                         throw new ArgumentNullException(nameof(orderRepository),
                             "Repository Cannot be Null.");
            _orderValidator = orderValidator ??
                              throw new ArgumentNullException(nameof(orderValidator),
                                  "Validator Cannot be Null.");
        }

        public Order Create(Order order)
        {
            order.DateCreated = DateTime.Now;
            _orderValidator.DefaultValidation(order);
            return _orderRepo.Create(order);
        }

        public List<Order> GetAll()
        {
            return _orderRepo.ReadAll().ToList();
        }

        public FilteredList<Order> GetAll(Filter filter)
        {
            throw new NotImplementedException();
        }
    }
}