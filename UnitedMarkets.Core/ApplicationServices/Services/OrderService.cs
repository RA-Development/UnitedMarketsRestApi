using System;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepo;
        private IOrderValidator _orderValidator;

        public OrderService(IOrderRepository orderRepository, IOrderValidator orderValidator)
        {
            _orderRepo = orderRepository ??
                         throw new ArgumentNullException(nameof(orderRepository),
                             "Repository Cannot be Null.");
            _orderValidator = orderValidator ??
                              throw new ArgumentNullException(nameof(orderValidator),
                                  "Validator Cannot be Null.");
        }

        public Order CreateOrder(Order order)
        {
            order.DateCreated = DateTime.Now;
            _orderValidator.DefaultValidation(order);
            return _orderRepo.CreateOrder(order);
        }
    }
}