using UnitedMarkets.Core.Entities.AuthenticationModels;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IUserService
    {
        string AuthenticateUser(LoginInputModel loginInputModel);
    }
}