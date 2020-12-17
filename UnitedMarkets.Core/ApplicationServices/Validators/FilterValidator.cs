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
            if (id < 1)
                throw new ArgumentException("MarketId cannot be less than 1.", nameof(id));
        }
    }
}