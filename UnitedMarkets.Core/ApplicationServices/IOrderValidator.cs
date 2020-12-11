using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IOrderValidator
    {
        void DefaultValidation(Order order);
    }
}