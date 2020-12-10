using System;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices.Validators
{
    public class ProductValidator : IValidator<Product>
    {
        public void DefaultValidation(Product product)
        {
            ValidateUnitPrice(product);
            ValidateAmount(product);
        }

        private void ValidateAmount(Product product)
        {
            if (product.Amount < 0)
                throw new ArgumentException("Positive value required for product amount.");
        }

        private void ValidateUnitPrice(Product product)
        {
            if (product.PricePerUnit < 0)
                throw new ArgumentException("Positive value required for product price.");
        }
    }
}