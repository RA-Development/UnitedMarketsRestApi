using System.Collections.Generic;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.DomainServices
{
    public interface IRepository<T>
    {
        T Create(T entity);
        IEnumerable<T> ReadAll();
        FilteredList<T> ReadAll(Filter filter);
        T ReadById(int id);
        T ReadByName(string name);
        T Update(T entity);
        T Delete(int id);
    }
}