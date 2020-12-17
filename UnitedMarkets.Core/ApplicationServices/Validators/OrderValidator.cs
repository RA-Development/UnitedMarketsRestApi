using System;
using System.Collections.Generic;
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
            ValidateStatus(order);
            ValidateProductList(order);
            ValidatePrice(order);
            ValidateBillingAddress(order);
            ValidateShippingAddress(order);
            ValidateProductId(order.Products);
            // TODO: Validate SubTotals add up to TotalPrice
        }

        public void IdValidation(int id)
        {
            if (id < 1) throw new ArgumentException("Id cannot be less than 1.", nameof(id));
        }

        public void CreateValidation(Order order)
        {
            ValidateDatesCreationValidation(order.DateCreated, order.DateUpdated);
            ValidatePendingStatus(order);
        }

        public void UpdateValidation(Order order)
        {
            ValidateDatesUpdateValidation(order);
            ValidateOrderIdOfOrderLines(order);
        }
        
        public void DeleteValidation(Order order)
        {
            if (!order.IsDeleted)
                throw new ArgumentException("Order has to be flagged as deleted.", nameof(order.IsDeleted));
        }

        private void ValidateStatus(Order order)
        {
            if (order.OrderStatusId < 1)
                throw new ArgumentException("OrderStatusId cannot be less than 1.", nameof(order.OrderStatus));
        }
        
        private void ValidateProductList(Order order)
        {
            if (order.Products == null)
                throw new ArgumentNullException(nameof(order.Products),
                    "Order must contain a list of products.");

            if (!order.Products.Any())
                throw new ArgumentException("List of products must contain at least one product.",
                    nameof(order.Products));
        }

        private void ValidatePrice(Order order)
        {
            if (order.TotalPrice == 0)
                throw new ArgumentException("Order cannot have a total price of 0.", nameof(order.TotalPrice));

            if (order.TotalPrice < 0)
                throw new ArgumentException("Order cannot have a total price of negative value.",
                    nameof(order.TotalPrice));
        }

        private void ValidateBillingAddress(Order order)
        {
            if (string.IsNullOrEmpty(order.BillingAddress))
                throw new ArgumentException("Order has to contain a billing address.", nameof(order.BillingAddress));
        }

        private void ValidateShippingAddress(Order order)
        {
            if (string.IsNullOrEmpty(order.ShippingAddress))
                throw new ArgumentException("Order has to contain a shipping address.", nameof(order.ShippingAddress));
        }

        private void ValidateProductId(IEnumerable<OrderLine> orderProducts)
        {
            var duplicates = orderProducts.Select(ol => ol.ProductId)
                .GroupBy(n => n).Any(c => c.Count() > 1);
            if (duplicates)
                throw new ArgumentException("ProductId in each order line has to be unique.", nameof(orderProducts));
        }

        private void ValidateDatesCreationValidation(DateTime dateCreated, DateTime dateUpdated)
        {
            var lowerBoundary = DateTime.Now.AddSeconds(-5);
            var upperBoundary = DateTime.Now.AddSeconds(5);

            if (dateCreated < lowerBoundary || dateCreated > upperBoundary)
                throw new ArgumentException(
                    "DateCreated for Order has to be within 5 seconds' precision.", nameof(dateCreated));
            if (dateUpdated < lowerBoundary || dateUpdated > upperBoundary)
                throw new ArgumentException(
                    "DateUpdated for Order has to be within 5 seconds' precision.", nameof(dateUpdated));
        }

        private void
            ValidatePendingStatus(Order order) // TODO: Status validation is vulnerable. Check for name instead?
        {
            if (order.OrderStatusId != 1)
                throw new ArgumentException(
                    "Order status has to be 'pending', with id = 1,  on creation.",
                    nameof(order.OrderStatusId));
        }

        private void ValidateDatesUpdateValidation(Order order)
        {
            ValidateDateCreated(order.DateCreated);
            ValidateDateUpdated(order.DateUpdated);
        }
        
        private void ValidateDateCreated(in DateTime dateCreated)
        {
            if (dateCreated.Equals(DateTime.MinValue))
                throw new ArgumentNullException(nameof(dateCreated), "Order must have a DateCreated.");
        }

        private void ValidateDateUpdated(DateTime dateUpdated)
        {
            var lowerBoundary = DateTime.Now.AddSeconds(-5);
            var upperBoundary = DateTime.Now.AddSeconds(5);
            if (dateUpdated < lowerBoundary || dateUpdated > upperBoundary)
                throw new ArgumentException(
                    "Order has to contain the current date within 5 seconds' precision for DateUpdated.",
                    nameof(dateUpdated));
        }
        
        private void ValidateOrderIdOfOrderLines(Order order)
        {
            if (order.Products.Any(orderLine => orderLine.OrderId != order.Id))
                throw new ArgumentException("OrderId of each OrderLine has to match with Id of Order.", nameof(order));
        }
    }
}