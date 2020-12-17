using System;
using FluentAssertions;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.Entities.AuthenticationModels;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Validators
{
    public class LoginInputModelValidatorTest
    {
        [Fact]
        public void LoginInputModelValidator_ShouldBeOfTypeIValidatorLoginInputModel()
        {
            new LoginInputModelValidator().Should().BeAssignableTo<IValidator<LoginInputModel>>();
        }

        [Theory]
        [InlineData(null, "Password")]
        [InlineData("   ", "Passw0rd")]
        public void DefaultValidation_WithNullUsername_ShouldThrowException(string username, string password)
        {
            IValidator<LoginInputModel> validator = new LoginInputModelValidator();
            Action action = () =>
                validator.DefaultValidation(new LoginInputModel {Username = username, Password = password});
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Username cannot be empty or whitespaces. (Parameter 'username')");
        }

        [Theory]
        [InlineData("Aluong", null)]
        [InlineData("Aluong", "   ")]
        public void DefaultValidation_WithNullPassword_ShouldThrowException(string username, string password)
        {
            IValidator<LoginInputModel> validator = new LoginInputModelValidator();
            Action action = () =>
                validator.DefaultValidation(new LoginInputModel {Username = username, Password = password});
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Password cannot be empty or whitespaces. (Parameter 'password')");
        }
    }
}