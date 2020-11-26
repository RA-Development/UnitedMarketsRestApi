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
        public void FilterValidator_ShouldBeOfTypeIFilterValidator()
        {
            new FilterValidator().Should().BeAssignableTo<IFilterValidator>();
        }


        [Fact]
        public void DefaultValidation_FilterWithNegativeId_ShouldThrowException()
        {
            IFilterValidator validator = new FilterValidator();
            var filter = new Filter() {MarketId = -4};
            Action action = () => validator.DefaultValidation(filter);
            action.Should().Throw<ArgumentException>()
                .WithMessage("MarketId has to be number bigger then 0.");
        }

        [Fact]
        public void DefaultValidation_FilterWithIdZero_ShouldThrowException()
        {
            IFilterValidator validator = new FilterValidator();
            var filter = new Filter() {MarketId = 0};
            Action action = () => validator.DefaultValidation(filter);
            action.Should().Throw<ArgumentException>()
                .WithMessage("MarketId has to be number bigger then 0.");
        }
    }
}