using System;
namespace Application.Validation
{
    public interface IValidator<T>
    {
        void Validate(T entity);
    }
}