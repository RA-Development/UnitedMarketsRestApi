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
        private readonly IValidatorExtended<OrderLine> _orderLineValidator;
        private readonly IRepository<Order> _orderRepository;
        private readonly IValidatorExtended<Order> _orderValidator;

        public OrderService(IRepository<Order> orderRepository,
            IValidatorExtended<Order> orderValidator,
            IValidatorExtended<OrderLine> orderLineValidator)
        {
            _orderRepository = orderRepository ??
                               throw new ArgumentNullException(nameof(orderRepository),
                                   "Repository cannot be null.");
            _orderValidator = orderValidator ??
                              throw new ArgumentNullException(nameof(orderValidator),
                                  "Validator cannot be null.");
            _orderLineValidator = orderLineValidator ??
                                  throw new ArgumentNullException(nameof(orderLineValidator),
                                      "Validator cannot be null.");
        }

        public Order Create(Order order)
        {
            order.DateCreated = DateTime.Now;
            order.DateUpdated = DateTime.Now;
            order.StatusId = 1;
            foreach (var orderLine in order.Products) _orderLineValidator.DefaultValidation(orderLine);
            _orderValidator.DefaultValidation(order);
            _orderValidator.CreateValidation(order);
            return _orderRepository.Create(order);
        }

        public List<Order> GetAll()
        {
            return _orderRepository.ReadAll().ToList();
        }

        public FilteredList<Order> GetAll(Filter filter)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order order)
        {
            _orderValidator.IdValidation(order.Id);
            order.DateUpdated = DateTime.Now;
            foreach (var orderLine in order.Products)
            {
                _orderLineValidator.DefaultValidation(orderLine);
                _orderLineValidator.UpdateValidation(orderLine);
            }

            _orderValidator.DefaultValidation(order);
            _orderValidator.UpdateValidation(order);
            return _orderRepository.Update(order);
        }

        public Order Delete(int id)
        {
            _orderValidator.IdValidation(id);
            var returnedOrder = _orderRepository.Delete(id);
            _orderValidator.DeleteValidation(returnedOrder);
            return returnedOrder;
        }
    }
}