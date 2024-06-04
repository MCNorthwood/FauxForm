using FauxForm.Engine.Interfaces.Models;

namespace FauxForm.Engine.Interfaces;
public interface IContactFormRepository : IGetAsync<IContactFormModel>, ISaveAsync<IContactFormModel>, IDeleteAsync<IContactFormModel>, ICacheKeyProvider;
