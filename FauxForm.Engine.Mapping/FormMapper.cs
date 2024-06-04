using FauxForm.Engine.Dtos;
using FauxForm.Engine.Interfaces.Mapping;
using FauxForm.Engine.Interfaces.Models;
using FauxForm.Engine.Models;

namespace FauxForm.Engine.Mapping;

public class FormMapper : IContactFormMapper
{
    public IContactFormDto Map(IContactFormModel? entity)
    {
        ContactFormDto dto = new ()
        {
            Email = entity?.Email ?? string.Empty,
            Name = entity?.Name ?? string.Empty,
            Phone = entity?.Phone ?? string.Empty,
            Enquiry = entity?.Enquiry ?? string.Empty
        };
        return dto;
    }

    public IContactFormModel Map(IContactFormDto? entity)
    {
        ContactForm model = new()
        {
            ID = new Random().Next(1, 10000),
            Email = entity?.Email ?? string.Empty,
            Name = entity?.Name ?? string.Empty,
            Phone = entity?.Phone ?? string.Empty,
            Enquiry = entity?.Enquiry ?? string.Empty
        };
        return model;
    }
}
