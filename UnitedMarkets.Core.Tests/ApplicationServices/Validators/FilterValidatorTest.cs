using System;
using FluentAssertions;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.Filtering;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Validators
{
    public class FilterValidatorTest
    {
        [Fact]
        public void FilterValidator_ShouldBeOfTypeIValidatorFilter()
        {
            new FilterValidator().Should().BeAssignableTo<IValidator<Filter>>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DefaultValidation_FilterWithZeroOrNegativeId_ShouldThrowException(int id)
        {
            IValidator<Filter> validator = new FilterValidator();
            var filter = new Filter {MarketId = id};
            Action action = () => validator.DefaultValidation(filter);
            action.Should().Throw<ArgumentException>()
                .WithMessage("MarketId cannot be less than 1. (Parameter 'id')");
        }
    }
}