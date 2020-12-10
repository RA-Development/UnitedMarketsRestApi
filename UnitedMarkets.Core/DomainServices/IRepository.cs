using System.Collections.Generic;

namespace UnitedMarkets.Core.DomainServices
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        T GetByName(string name);
        void Create(T entity);
        void Update(T entity);
        void Delete(long id);
    }
}