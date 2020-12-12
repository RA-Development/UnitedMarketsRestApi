using System;
using FluentAssertions;
using Moq;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.HelperServices;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities.AuthenticationModels;
using Xunit;

namespace UnitedMarkets.Core.Tests.ApplicationServices.Services
{
    public class UserServiceTest
    {
        private IAuthenticationHelper _authenticationHelper;
        private Mock<IRepository<User>> _repositoryMock;
        private User _user;
        private IUserService _userService;
        private Mock<IValidator<LoginInputModel>> _validatorMock;

        public UserServiceTest()
        {
            ArrangeAuthenticateUserTests();
        }

        private void ArrangeAuthenticateUserTests()
        {
            _repositoryMock = new Mock<IRepository<User>>();
            _validatorMock = new Mock<IValidator<LoginInputModel>>();

            var secretBytes = new byte[40];
            var rand = new Random();
            rand.NextBytes(secretBytes);
            _authenticationHelper = new AuthenticationHelper(secretBytes);

            _authenticationHelper.CreatePasswordHash("Passw0rd", out var passwordHash, out var passwordSalt);

            _user = new User
            {
                Username = "Aluong",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsAdmin = true
            };

            _repositoryMock.Setup(repo => repo.ReadByName(_user.Username)).Returns(_user);

            _userService = new UserService(_repositoryMock.Object, _authenticationHelper,
                _validatorMock.Object);
        }

        [Fact]
        public void UserService_ShouldBeOfTypeIUserService()
        {
            new UserService(_repositoryMock.Object, _authenticationHelper, _validatorMock.Object)
                .Should()
                .BeAssignableTo<IUserService>();
        }

        [Fact]
        public void NewUserService_WithNullRepository_ShouldThrowException()
        {
            Action action = () =>
                new UserService(null, _authenticationHelper, _validatorMock.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository cannot be null. (Parameter 'userRepository')");
        }

        [Fact]
        public void NewUserService_WithNullAuthenticationHelper_ShouldThrowException()
        {
            Action action = () => new UserService(_repositoryMock.Object, null, _validatorMock.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("AuthenticationHelper cannot be null. (Parameter 'authenticationHelper')");
        }

        [Fact]
        public void NewUserService_WithNullValidator_ShouldThrowException()
        {
            Action action = () => new UserService(_repositoryMock.Object, _authenticationHelper, null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("LoginInputModelValidator cannot be null. (Parameter 'loginInputModelValidator')");
        }

        [Fact]
        public void AuthenticateUser_ShouldCallLoginInputModelValidatorDefaultValidationWithLoginInputModelParam_Once()
        {
            //Act
            var loginInputModel = new LoginInputModel {Username = "Aluong", Password = "Passw0rd"};
            _userService.AuthenticateUser(loginInputModel);

            //Assert
            _validatorMock.Verify(validator => validator.DefaultValidation(loginInputModel), Times.Once);
        }

        [Fact]
        public void AuthenticateUser_ShouldCallUserRepositoryReadByNameWithTheLoginInputModelAsParam_Once()
        {
            //Act
            var loginInputModel = new LoginInputModel {Username = "Aluong", Password = "Passw0rd"};
            _userService.AuthenticateUser(loginInputModel);

            //Assert
            _repositoryMock.Verify(repository => repository.ReadByName(_user.Username), Times.Once);
        }

        [Theory]
        [InlineData("Aluong", "Password")]
        [InlineData("anneluong", "Passw0rd")]
        public void AuthenticateUser_WithInvalidUsernameOrPassword_ShouldThrowException(string username,
            string password)
        {
            //Act
            var loginInputModel = new LoginInputModel {Username = username, Password = password};
            Action action = () => _userService.AuthenticateUser(loginInputModel);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(username.Equals("Aluong")
                ? "Invalid password."
                : "Non-existing username.");
        }
    }
}