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
        [Fact]
        public void NewUserService_WithNullRepository_ShouldThrowException()
        {
            var authenticationHelperMock = new Mock<IAuthenticationHelper>();
            Action action = () => new UserService(null, authenticationHelperMock.Object);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("Repository cannot be null. (Parameter 'userRepository')");
        }

        [Fact]
        public void NewUserService_WithNullAuthenticationHelper_ShouldThrowException()
        {
            var repositoryMock = new Mock<IRepository<User>>();
            Action action = () => new UserService(repositoryMock.Object, null);
            action.Should().Throw<ArgumentNullException>()
                .WithMessage("AuthenticationHelper cannot be null. (Parameter 'authenticationHelper')");
        }

        [Fact]
        public void UserService_ShouldBeOfTypeIUserService()
        {
            var repositoryMock = new Mock<IRepository<User>>();
            var authenticationHelperMock = new Mock<IAuthenticationHelper>();
            new UserService(repositoryMock.Object, authenticationHelperMock.Object).Should()
                .BeAssignableTo<IUserService>();
        }

        [Fact]
        public void ValidateUser_ShouldCallUserRepositoryGetByNameWithTheLoginInputModelAsParam_Once()
        {
            //Arrange
            var repositoryMock = new Mock<IRepository<User>>();

            var secretBytes = new byte[40];
            var rand = new Random();
            rand.NextBytes(secretBytes);
            IAuthenticationHelper authenticationHelper = new AuthenticationHelper(secretBytes);

            authenticationHelper.CreatePasswordHash("Passw0rd", out var passwordHash, out var passwordSalt);

            var user = new User
            {
                Username = "Aluong",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsAdmin = true
            };

            repositoryMock.Setup(repo => repo.GetByName(user.Username)).Returns(user);

            IUserService userService = new UserService(repositoryMock.Object, authenticationHelper);

            //Act
            var loginInputModel = new LoginInputModel {Username = "Aluong", Password = "Passw0rd"};
            userService.ValidateUser(loginInputModel);

            //Assert
            repositoryMock.Verify(repo => repo.GetByName(user.Username), Times.Once);
        }

        [Theory]
        [InlineData("Aluong", "Password")]
        [InlineData("anneluong", "Passw0rd")]
        public void ValidateUser_WithInvalidUsernameOrPassword_ShouldThrowException(string username, string password)
        {
            //Arrange
            var repositoryMock = new Mock<IRepository<User>>();

            var secretBytes = new byte[40];
            var rand = new Random();
            rand.NextBytes(secretBytes);
            IAuthenticationHelper authenticationHelper = new AuthenticationHelper(secretBytes);

            authenticationHelper.CreatePasswordHash("Passw0rd", out var passwordHash, out var passwordSalt);

            var user = new User
            {
                Username = "Aluong",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsAdmin = true
            };

            repositoryMock.Setup(repo => repo.GetByName(user.Username)).Returns(user);

            IUserService userService = new UserService(repositoryMock.Object, authenticationHelper);

            //Act
            var loginInputModel = new LoginInputModel {Username = username, Password = password};
            Action action = () => userService.ValidateUser(loginInputModel);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(username.Equals("Aluong")
                ? "Invalid password."
                : "Non-existing username.");
        }
    }
}