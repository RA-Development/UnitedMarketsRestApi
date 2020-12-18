using System;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Validators
{
    public class OrderLineValidator : IValidatorExtended<OrderLine>
    {
        public void DefaultValidation(OrderLine orderLine)
        {
            ValidateProductId(orderLine);
            ValidateQuantity(orderLine);
            ValidateSubTotal(orderLine);
        }

        public void IdValidation(int id)
        {
            throw new NotImplementedException();
        }

        public void CreateValidation(OrderLine orderLine)
        {
            throw new NotImplementedException();
        }

        public void UpdateValidation(OrderLine orderLine)
        {
            ValidateOrderId(orderLine);
        }

        public void DeleteValidation(OrderLine entity)
        {
            throw new NotImplementedException();
        }
        
        private void ValidateProductId(OrderLine orderLine)
        {
            if (orderLine.ProductId == 0)
                throw new ArgumentException("ProductId cannot be 0 in OrderLine.", nameof(orderLine.ProductId));

            if (orderLine.ProductId < 0)
                throw new ArgumentException("ProductId cannot be a negative value in OrderLine.",
                    nameof(orderLine.ProductId));
        }

        private void ValidateQuantity(OrderLine orderLine)
        {
            if (orderLine.Quantity == 0)
                throw new ArgumentException("Quantity cannot be 0 in OrderLine.", nameof(orderLine.Quantity));

            if (orderLine.Quantity < 0)
                throw new ArgumentException("Quantity cannot be a negative value in OrderLine.",
                    nameof(orderLine.Quantity));
        }

        private void ValidateSubTotal(OrderLine orderLine)
        {
            if (orderLine.SubTotalPrice == 0)
                throw new ArgumentException("SubTotalPrice cannot be 0 in OrderLine.", nameof(orderLine.SubTotalPrice));

            if (orderLine.SubTotalPrice < 0)
                throw new ArgumentException("SubTotalPrice cannot be a negative value in OrderLine.",
                    nameof(orderLine.SubTotalPrice));
        }
        
        private void ValidateOrderId(OrderLine orderLine)
        {
            if (orderLine.OrderId == 0)
                throw new ArgumentException("OrderId cannot be 0 in OrderLine.", nameof(orderLine.OrderId));
            if (orderLine.OrderId < 0)
                throw new ArgumentException("OrderId cannot be a negative value in OrderLine.",
                    nameof(orderLine.OrderId));
        }
    }
}