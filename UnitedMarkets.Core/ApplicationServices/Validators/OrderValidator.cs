using System;
using System.Globalization;
using System.Linq;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Validators
{
    public class OrderValidator : IOrderValidator
    {
        public void DefaultValidation(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order),
                    "Order Cannot be Null.");

            ValidateDateCreated(order.DateCreated);
            ValidateProductList(order);
            ValidatePrice(order);
            ValidateBillingAddress(order);
            ValidateShippingAddress(order);
            ValidatePendingStatus(order);
        }

        private void ValidateDateCreated(DateTime dateCreated)
        {
            var startDate = DateTime.Now.AddSeconds(-5);
            var endDate = DateTime.Now.AddSeconds(5);

            if (dateCreated < startDate)
                throw new ArgumentException("Order creation date is set to past value. " +
                                            "Please input current date with 5 second precision.");
            if (dateCreated < startDate || dateCreated > endDate)
                throw new ArgumentException("Order creation date is set to future value. " +
                                            "Please input current date with 5 second precision.");
        }


        private void ValidatePendingStatus(Order order)
        {
            if (order.OrderStatusId != 4)
                throw new ArgumentException("Order status has to be 'pending' on creation. " +
                                            "(Pending status id = 4)");
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