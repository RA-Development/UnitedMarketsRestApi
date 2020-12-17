namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IValidator<in T>
    {
        void DefaultValidation(T entity);
    }
}