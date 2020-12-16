using System;
using System.Collections.Generic;
using System.Linq;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Validators
{
    public class OrderValidator : IValidator<Order>
    {
        public void DefaultValidation(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order),
                    "Order Cannot be Null.");
            ValidateProductList(order);
            ValidatePrice(order);
            ValidateBillingAddress(order);
            ValidateShippingAddress(order);
            ValidateProductIdDuplicates(order.Products);
        }

        private void ValidateProductIdDuplicates(IEnumerable<OrderLine> orderProducts)
        {
            var duplicates = orderProducts.Select(ol => ol.ProductId)
                .GroupBy(n => n).Any(c => c.Count() > 1);
            if (duplicates)
                throw new ArgumentException("Product Id in each order line has to be unique.");
        }


        public void UpdateValidation(Order order)
        {
            ValidateDateUpdated(order.DateUpdated);
            ValidateStatus(order); // move to default val
            ValidateOrderIdOfOrderlines(order);
        }

        private void ValidateOrderIdOfOrderlines(Order order)
        {
            if (order.Products.Any(ol => ol.OrderId != order.Id))
                throw new ArgumentException("OrderId of each order line has to match with order id.");
        }

        private void ValidateStatus(Order order)
        {
            if (order.OrderStatusId == 0)
                throw new ArgumentException("Order status id cannot be 0.");

            if (order.OrderStatusId < 0)
                throw new ArgumentException("Order status id cannot be negative value.");
        }

        public void CreateValidation(Order order)
        {
            ValidateDates(order.DateCreated, order.DateUpdated);
            ValidatePendingStatus(order);
        }

        private void ValidateDateUpdated(DateTime dateUpdated)
        {
            var startDate = DateTime.Now.AddSeconds(-5);
            var endDate = DateTime.Now.AddSeconds(5);
            if (dateUpdated < startDate || dateUpdated > endDate)
            {
                throw new ArgumentException("Order has to contain current date for dateUpdated. " +
                                            "Please input current date with 5 second precision.");
            }
        }

        private void ValidateDates(DateTime dateCreated, DateTime dateUpdated)
        {
            var startDate = DateTime.Now.AddSeconds(-5);
            var endDate = DateTime.Now.AddSeconds(5);

            if (dateCreated < startDate || dateCreated > endDate)
                throw new ArgumentException(
                    "Incorrect dateCreated for order. Set current date with 5 seconds precision.");
            if (dateUpdated < startDate || dateUpdated > endDate)
                throw new ArgumentException(
                    "Incorrect dateUpdated for order. Set current date with 5 seconds precision.");
        }


        private void ValidatePendingStatus(Order order)
        {
            if (order.OrderStatusId != 1)
                throw new ArgumentException(
                    "Order status has to be 'pending' on creation. (Pending status id = 1)");
        }

        private void ValidateShippingAddress(Order order)
        {
            if (string.IsNullOrEmpty(order.ShippingAddress))
                throw new ArgumentException("Order has to contain shipping address.");
        }

        private void ValidateBillingAddress(Order order)
        {
            if (string.IsNullOrEmpty(order.BillingAddress))
                throw new ArgumentException("Order has to contain billing address.");
        }

        private void ValidatePrice(Order order)
        {
            if (order.TotalPrice == 0)
                throw new ArgumentException("Order total price Cannot be 0.");

            if (order.TotalPrice < 0)
                throw new ArgumentException("Order total price Cannot be negative value.");
        }

        private void ValidateProductList(Order order)
        {
            if (order.Products == null)
                throw new ArgumentNullException(nameof(order.Products),
                    "Order must contain product list.");

            if (!order.Products.Any())
                throw new ArgumentException("Product list must contain at least one product.");
        }
    }
}