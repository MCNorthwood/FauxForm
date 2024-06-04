namespace FauxForm.Engine.Interfaces;
public interface ICacheKeyProvider
{
    public string? Key { get; }
    public void SetKey(string key);
}
