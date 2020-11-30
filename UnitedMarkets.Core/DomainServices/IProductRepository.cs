using System;
using System.Collections.Generic;
using System.Text;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.DomainServices
{
    public interface IProductRepository
    {
        FilteredList<Product> GetAllProducts(Filter filter);
    }
}