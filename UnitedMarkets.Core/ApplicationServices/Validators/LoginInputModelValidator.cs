using System;
using UnitedMarkets.Core.Entities.AuthenticationModels;

namespace UnitedMarkets.Core.ApplicationServices.Validators
{
    public class LoginInputModelValidator : IValidator<LoginInputModel>
    {
        public void DefaultValidation(LoginInputModel loginInputModel)
        {
            ValidateUsername(loginInputModel.Username);
            ValidatePassword(loginInputModel.Password);
        }

        public void UpdateValidation(LoginInputModel entity)
        {
            throw new NotImplementedException();
        }

        public void CreateValidation(LoginInputModel entity)
        {
            throw new NotImplementedException();
        }

        private void ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Username cannot be empty or whitespaces.");
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be empty or whitespaces.");
        }
    }
}