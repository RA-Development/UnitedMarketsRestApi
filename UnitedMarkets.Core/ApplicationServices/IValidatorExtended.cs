namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IValidatorExtended<in T> : IValidator<T>
    {
        void IdValidation(int id);
        void CreateValidation(T entity);
        void UpdateValidation(T entity);
        void DeleteValidation(T entity);
    }
}