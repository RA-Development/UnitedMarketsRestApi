using System;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Validators
{
    public class OrderLineValidator : IValidator<OrderLine>
    {
        public void DefaultValidation(OrderLine orderLine)
        {
            
            ValidateProductId(orderLine);
            ValidateQuantity(orderLine);
            ValidateSubTotal(orderLine);
        }

        

        private void ValidateOrderId(OrderLine orderLine)
        {
            if (orderLine.OrderId == 0)
                throw new ArgumentException("Order line order id cannot be 0.");

            if (orderLine.OrderId < 0)
                throw new ArgumentException("Order line order id cannot be negative value.");
        }

        private void ValidateSubTotal(OrderLine orderLine)
        {
            if (orderLine.SubTotalPrice == 0)
                throw new ArgumentException("Order line sub total price cannot be 0.");

            if (orderLine.SubTotalPrice < 0)
                throw new ArgumentException("Order line sub total price cannot be negative value.");
        }

        private void ValidateQuantity(OrderLine orderLine)
        {
            if (orderLine.Quantity == 0)
                throw new ArgumentException("Order line quantity cannot be 0.");

            if (orderLine.Quantity < 0)
                throw new ArgumentException("Order line quantity cannot be negative value.");
        }

        private void ValidateProductId(OrderLine orderLine)
        {
            if (orderLine.ProductId == 0)
                throw new ArgumentException("Product id cannot be 0.");

            if (orderLine.ProductId < 0)
                throw new ArgumentException("Product id cannot be negative value.");
        }

        public void UpdateValidation(OrderLine orderLine)
        {
            ValidateOrderId(orderLine);
        }

        public void CreateValidation(OrderLine orderLine)
        {
            throw new System.NotImplementedException();
        }
    }
}