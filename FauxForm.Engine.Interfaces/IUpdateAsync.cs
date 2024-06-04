namespace FauxForm.Engine.Interfaces;
public interface IUpdateAsync<T> where T : class
{
    Task UpdateAsync(T entity, CancellationToken token);
}
