using FauxForm.Engine.Interfaces;
using FauxForm.Engine.Interfaces.Mapping;
using FauxForm.Engine.Interfaces.Models;
using FluentValidation;

namespace FauxForm.Engine.Services;

public class ContactFormService(IContactFormRepository _contactFormRepository, IContactFormValidation _validator, IContactFormMapper _mapper) : IContactFormService
{
    public async Task<IContactFormDto> GetAsync(string name, CancellationToken token)
    {
        return _mapper.Map(await _contactFormRepository.GetAsync(name, token));
    }

    public async Task SaveAsync(IContactFormDto entity, CancellationToken token)
    {
        IContactFormModel model = _mapper.Map(entity);

        await _validator.ValidateAndThrowAsync(model, token);

        await _contactFormRepository.SaveAsync(model, token);
    }

    public async Task UpdateAsync(IContactFormDto entity, CancellationToken token)
    {
        Task update;
        IContactFormModel model = _mapper.Map(entity);

        await _validator.ValidateAndThrowAsync(model, token);

        var formExists = await _contactFormRepository.GetAsync(model?.Name ?? string.Empty, token);

        update = formExists != null ? _contactFormRepository.SaveAsync(model, token) : throw new ValidationException("Form does not exist to be updated");

        await update;
    }

    public async Task DeleteAsync(IContactFormDto entity, CancellationToken token)
    {
        Task delete;
        IContactFormModel model = _mapper.Map(entity);

        await _validator.ValidateAndThrowAsync(model, token);

        var formExists = await _contactFormRepository.GetAsync(model?.Name ?? string.Empty, token);

        delete = formExists != null ? _contactFormRepository.DeleteAsync(formExists, token) : throw new ValidationException("Form does not exist to be deleted");

        await delete;
    }
}
