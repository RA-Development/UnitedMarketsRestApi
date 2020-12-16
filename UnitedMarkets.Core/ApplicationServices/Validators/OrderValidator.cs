using System;
using System.Linq;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Validators
{
    public class OrderValidator : IValidatorExtended<Order>
    {
        public void DefaultValidation(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order),
                    "Order cannot be null.");
            
            ValidateProductList(order);
            ValidatePrice(order);
            ValidateBillingAddress(order);
            ValidateShippingAddress(order);
        }

        public void IdValidation(long id)
        {
            if (id < 1) throw new ArgumentException("Id cannot be less than 1.");
        }

        public void StatusValidation(Order order, string requiredStatus)
        {
            if (order.OrderStatus == null)
            {
                throw new ArgumentNullException(nameof(order.OrderStatus),"Status cannot be null.");
            }
            if (!order.OrderStatus.Name.Equals(requiredStatus))
            {
                throw new ArgumentException("Status should be \""+requiredStatus +"\".");
            }
        }
        
        public void DateCreatedValidation(Order order)
        {
            var upperBoundary = DateTime.Now.AddSeconds(-5);
            var lowerBoundary = DateTime.Now.AddSeconds(5);
            
            if (order.DateCreated < upperBoundary || order.DateCreated > lowerBoundary)
                throw new ArgumentException("Order creation date is not within 5 second precision.");
        }
        
        private void ValidateProductList(Order order)
        {
            if (order.Products == null)
                throw new ArgumentNullException(nameof(order.Products),
                    "Order must contain product list.");

            if (!order.Products.Any())
                throw new ArgumentException("Product list must contain at least one product.");
        }
        
        private void ValidatePrice(Order order)
        {
            if (order.TotalPrice == 0)
                throw new ArgumentException("Order total price Cannot be 0.");

            if (order.TotalPrice < 0)
                throw new ArgumentException("Order total price Cannot be negative value.");
        }
        
        private void ValidateBillingAddress(Order order)
        {
            if (string.IsNullOrEmpty(order.BillingAddress))
                throw new ArgumentException("Order has to contain billing address.");
        }
        
        private void ValidateShippingAddress(Order order)
        {
            if (string.IsNullOrEmpty(order.ShippingAddress))
                throw new ArgumentException("Order has to contain shipping address.");
        }
    }
}