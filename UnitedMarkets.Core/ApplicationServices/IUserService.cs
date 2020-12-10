using UnitedMarkets.Core.Entities.AuthenticationModels;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IUserService
    {
        string ValidateUser(LoginInputModel loginInputModel);
    }
}