using System;
using UnitedMarkets.Core.ApplicationServices.HelperServices;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities.AuthenticationModels;

namespace UnitedMarkets.Core.ApplicationServices.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IValidator<LoginInputModel> _loginInputModelValidator;
        private readonly IRepository<User> _userRepository;

        public UserService(
            IRepository<User> userRepository,
            IAuthenticationHelper authenticationHelper,
            IValidator<LoginInputModel> loginInputModelValidator)
        {
            _userRepository = userRepository ??
                              throw new ArgumentNullException(nameof(userRepository),
                                  "Repository cannot be null.");
            _authenticationHelper = authenticationHelper ??
                                    throw new ArgumentNullException(nameof(authenticationHelper),
                                        "AuthenticationHelper cannot be null.");
            _loginInputModelValidator = loginInputModelValidator ??
                                        throw new ArgumentNullException(nameof(loginInputModelValidator),
                                            "LoginInputModelValidator cannot be null.");
        }

        public string AuthenticateUser(LoginInputModel loginInputModel)
        {
            _loginInputModelValidator.DefaultValidation(loginInputModel);
            var user = FindUser(loginInputModel.Username);

            if (!_authenticationHelper.VerifyPasswordHash(loginInputModel.Password, user.PasswordHash,
                user.PasswordSalt)) throw new ArgumentException("Invalid password.");

            return _authenticationHelper.GenerateToken(user);
        }

        private User FindUser(string username)
        {
            var user = _userRepository.GetByName(username);

            if (user == null) throw new ArgumentException("Non-existing username.");

            return user;
        }
    }
}