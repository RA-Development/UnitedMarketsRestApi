using System;

namespace UnitedMarkets.Core.ApplicationServices
{
    public interface IValidatorExtended<in T>: IValidator<T>
    {
        void IdValidation(long id);
        void StatusValidation(T entity, string requiredStatus);
        void DateCreatedValidation(T entity);
    }
}