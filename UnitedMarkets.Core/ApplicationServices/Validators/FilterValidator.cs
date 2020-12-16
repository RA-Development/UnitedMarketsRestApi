using System;
using UnitedMarkets.Core.Filtering;

namespace UnitedMarkets.Core.ApplicationServices.Validators
{
    public class FilterValidator : IValidator<Filter>
    {
        public void DefaultValidation(Filter filter)
        {
            ValidateMarketId(filter.MarketId);
        }

        public void UpdateValidation(Filter entity)
        {
            throw new NotImplementedException();
        }

        public void CreateValidation(Filter entity)
        {
            throw new NotImplementedException();
        }

        private void ValidateMarketId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("MarketId has to be number bigger then 0.");
        }
    }
}