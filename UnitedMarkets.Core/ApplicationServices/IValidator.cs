namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IValidator<in T>
    {
        void DefaultValidation(T entity);

        void UpdateValidation(T entity);

        void CreateValidation(T entity);
    }
}