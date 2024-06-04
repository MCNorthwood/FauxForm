namespace FauxForm.Engine.Interfaces;
public interface IGetAsync<T> where T : class
{
    Task<T> GetAsync(string name, CancellationToken token);
}
