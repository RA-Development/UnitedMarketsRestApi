using System.Collections.Generic;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IService<T>
    {
        List<T> GetAll();
    }
}