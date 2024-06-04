using FauxForm.Engine.Interfaces;
using FauxForm.Engine.Interfaces.Models;
using FluentValidation;

namespace FauxForm.Engine.Validation;

public class ContactFormValidation : AbstractValidator<IContactForm>, IContactFormValidation
{
    public ContactFormValidation()
    {
        RuleFor(x => x.Name).NotEmpty().Matches("^[A-Za-z\\s]*$");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Phone).NotEmpty().Matches("^(?:0|\\+?44)(?:\\d\\s?){9,10}$");
        RuleFor(x => x.Enquiry).NotEmpty().MaximumLength(250);
    }

    public async Task ValidateAndThrowAsync(IContactForm entity, CancellationToken token)
    {
        var instance = ValidationContext<IContactForm>.CreateWithOptions(entity, options =>
        {
            options.ThrowOnFailures();
        });
        await ValidateAsync(instance, token);
    }
}
