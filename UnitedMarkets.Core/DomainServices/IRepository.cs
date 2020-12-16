using System.Collections.Generic;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.DomainServices
{
    public interface IRepository<T>
    {
        IEnumerable<T> ReadAll();
        T ReadById(int id);
        T ReadByName(string name);
        T Create(T entity);
        T Update(T entity);
        T Delete(int id);
        FilteredList<Product> ReadAll(Filter filter);
    }
}