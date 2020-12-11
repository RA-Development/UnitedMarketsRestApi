using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IOrderService
    {
        Order CreateOrder(Order order);
    }
}