using System;
using System.Collections.Generic;
using System.Text;
using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IProductValidator
    {
        void DefaultValidation(Product product);
    }
}