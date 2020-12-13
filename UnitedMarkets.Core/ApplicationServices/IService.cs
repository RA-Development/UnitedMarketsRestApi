using System.Collections.Generic;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IService<T>
    {
        List<T> GetAll();

        FilteredList<T> GetAll(Filter filter);

        T Create(T entity);
    }
}