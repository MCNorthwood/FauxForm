using FauxForm.Engine.Validation;
using FauxForm.Engine.Models;
using FluentAssertions;
using FluentValidation;

namespace FauxForm.UnitTests;
public class ContactFormValidationTest
{
    private readonly ContactFormValidation _validator;
    private readonly CancellationToken _token;

    public ContactFormValidationTest()
    {
        _validator = new ContactFormValidation();
        _token = new CancellationToken();
    }

    [Theory]
    [InlineData("Bob Ross", "bobross@example.com", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Matthew Northwood", "bobross@example.net", "+441915555555", "q")]
    [InlineData("King Charles II", "bobross@example.co.uk", "+447777777777", "q")]
    public async Task When_a_valid_contact_form_is_valid_validator_does_not_throw_exception(string name, string email, string phoneNumber, string enquiry)
    {
        var act = () => _validator.ValidateAndThrowAsync(new ContactForm { Name = name, Email = email, Phone = phoneNumber, Enquiry = enquiry }, _token);

        await act.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData("Bob Ross", "bobross@example.com", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Matthew Northwood", "bobross@example.net", "+441915555555", "q")]
    [InlineData("King Charles II", "bobross@example.co.uk", "+447777777777", "q")]
    public void When_a_valid_contact_form_is_valid_validator_returns_true(string name, string email, string phoneNumber, string enquiry)
    {
        var result = _validator.Validate(new ContactForm { Name = name, Email = email, Phone = phoneNumber, Enquiry = enquiry });

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void When_contact_form_is_empty_the_validator_returns_false()
    {
        var result = _validator.Validate(new ContactForm());

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task When_contact_form_is_empty_validator_throws_exception()
    {
        var act = () => _validator.ValidateAndThrowAsync(new ContactForm(), _token);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Theory]
    [InlineData("", "bobross@example.com", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobrossexample", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobross@example.com", "0191", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobross@example.com", "01915555555", "")]
    [InlineData("Bob Ross", "bobrossexample.com", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobross@example.com", "01915555555", "qwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnm")]
    public void When_a_contact_form_is_invalid_validator_returns_false_with_one_error(string name, string email, string phoneNumber, string enquiry)
    {
        var result = _validator.Validate(new ContactForm { Name = name, Email = email, Phone = phoneNumber, Enquiry = enquiry });

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Should().HaveCount(1);
    }

    [Theory]
    [InlineData("Bob Ross", "", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobross@example.com", "", "qwertyuiopasdfghjklzxcvbn")]
    public void When_a_contact_form_is_invalid_validator_returns_false_with_two_errors(string name, string email, string phoneNumber, string enquiry)
    {
        var result = _validator.Validate(new ContactForm { Name = name, Email = email, Phone = phoneNumber, Enquiry = enquiry });

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Should().HaveCount(2);
    }

    [Theory]
    [InlineData("", "bobross@example.com", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobrossexample", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobross@example.com", "0191", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobross@example.com", "01915555555", "")]
    [InlineData("Bob Ross", "bobrossexample.com", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "", "01915555555", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobross@example.com", "", "qwertyuiopasdfghjklzxcvbn")]
    [InlineData("Bob Ross", "bobross@example.com", "01915555555", "qwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnm")]
    public async Task When_a_contact_form_is_invalid_validator_returns_false_with_validation_exception(string name, string email, string phoneNumber, string enquiry)
    {
        var act = () => _validator.ValidateAndThrowAsync(new ContactForm { Name = name, Email = email, Phone = phoneNumber, Enquiry = enquiry }, _token);

        await act.Should().ThrowAsync<ValidationException>();
    }
}
