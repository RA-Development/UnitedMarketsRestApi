using System.Collections.Generic;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IService<T>
    {
        T Create(T entity);
        List<T> GetAll();
        FilteredList<T> GetAll(Filter filter);
        T Update(T entity);
        T Delete(int id);
    }
}