namespace FauxForm.Engine.Interfaces;

public interface ISaveAsync<T> where T : class
{
    Task SaveAsync(T entity, CancellationToken token);
}
