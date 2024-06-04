namespace FauxForm.Engine.Interfaces;
public interface IDeleteAsync<T> where T : class
{
    Task DeleteAsync(T entity, CancellationToken token);
}
