using FauxForm.Engine.Interfaces.Models;

namespace FauxForm.Engine.Interfaces;
public interface IContactFormService : IGetAsync<IContactFormDto>, ISaveAsync<IContactFormDto>, IDeleteAsync<IContactFormDto> , IUpdateAsync<IContactFormDto>;
