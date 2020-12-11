using UnitedMarkets.Core.Entities;

namespace UnitedMarkets.Core.DomainServices
{
    public interface IOrderRepository
    {
        Order CreateOrder(Order order);
    }
}