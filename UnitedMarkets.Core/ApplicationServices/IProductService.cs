using System;
using System.Collections.Generic;
using System.Text;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IProductService
    {
        FilteredList<Product> GetAllProducts(Filter filter);
    }
}
