namespace FauxForm.Engine.Interfaces;
public interface IValidation<T>
{
    Task ValidateAndThrowAsync(T entity, CancellationToken token);
}
