using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IFilterValidator
    {
        void DefaultValidation(Filter filter);
    }
}