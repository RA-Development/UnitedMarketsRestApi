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
        private IValidator<OrderLine> _olValidator;

        public OrderService(IRepository<Order> orderRepository,
            IValidator<Order> orderValidator,
            IValidator<OrderLine> orderLineValidator)
        {
            _orderRepo = orderRepository ??
                         throw new ArgumentNullException(nameof(orderRepository),
                             "Repository Cannot be Null.");
            _orderValidator = orderValidator ??
                              throw new ArgumentNullException(nameof(orderValidator),
                                  "Validator Cannot be Null.");
            _olValidator = orderLineValidator ??
                           throw new ArgumentNullException(nameof(orderLineValidator),
                               "Validator Cannot be Null.");
        }

        public Order Create(Order order)
        {
            order.DateCreated = DateTime.Now;
            order.DateUpdated = DateTime.Now;
            order.OrderStatusId = 1;
            foreach (var ol in order.Products)
            {
                _olValidator.DefaultValidation(ol);
            }

            _orderValidator.DefaultValidation(order);
            _orderValidator.CreateValidation(order);
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

        public Order Update(Order order)
        {
            order.DateUpdated = DateTime.Now;
            foreach (var ol in order.Products)
            {
                _olValidator.DefaultValidation(ol);
            }

            _orderValidator.DefaultValidation(order);
            _orderValidator.UpdateValidation(order);
            return _orderRepo.Update(order);
        }
    }
}