using FauxForm.Engine.Interfaces;
using FauxForm.Engine.Interfaces.Models;
using FauxForm.Engine.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FauxForm.Engine.Repository;
public class ContactFormRepository(IDistributedCache cache) : IContactFormRepository
{
    public string? Key { get; private set; }

    public async Task<IContactFormModel> GetAsync(string name, CancellationToken token)
    {
        SetKey(name);
        var jsonData = await cache.GetStringAsync(Key ?? string.Empty, token) ?? string.Empty;

        return JsonConvert.DeserializeObject<ContactForm>(jsonData) ?? null;
    }

    public async Task SaveAsync(IContactFormModel entity, CancellationToken token)
    {
        SetKey(entity.Name);
        var jsonData = JsonConvert.SerializeObject(entity);

        await cache.SetStringAsync(Key ?? string.Empty, jsonData, token);
    }

    public async Task DeleteAsync(IContactFormModel entity, CancellationToken token)
    {
        SetKey(entity.Name);
        await cache.RemoveAsync(Key ?? string.Empty, token);
    }

    public void SetKey(string key)
    {
        Key = $"{GetType().Name}-{key}";
    }
}
