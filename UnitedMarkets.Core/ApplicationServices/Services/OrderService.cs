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
        private readonly IRepository<Order> _orderRepository;
        private readonly IValidatorExtended<Order> _orderValidator;

        public OrderService(IRepository<Order> orderRepository, IValidatorExtended<Order> orderValidator)
        {
            _orderRepository = orderRepository ??
                         throw new ArgumentNullException(nameof(orderRepository),
                             "Repository cannot be null.");
            _orderValidator = orderValidator ??
                              throw new ArgumentNullException(nameof(orderValidator),
                                  "Validator cannot be null.");
        }

        public Order Create(Order order)
        {
            order.DateCreated = DateTime.Now;
            _orderValidator.DefaultValidation(order);
            //_orderValidator.StatusValidation(order,"Pending");
            _orderValidator.DateCreatedValidation(order);
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

        public Order Delete(int id)
        {
            _orderValidator.IdValidation(id);
            var returnedOrder = _orderRepository.Delete(id);
            _orderValidator.IsDeletedValidation(returnedOrder);
            return returnedOrder;
        }
    }
}