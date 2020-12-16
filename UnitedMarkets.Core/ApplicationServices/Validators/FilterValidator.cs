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

        private void ValidateMarketId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("MarketId has to be greater than 0.");
        }
    }
}