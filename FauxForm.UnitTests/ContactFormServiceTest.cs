using FauxForm.Engine.Interfaces;
using FauxForm.Engine.Interfaces.Models;
using FauxForm.Engine.Services;
using FauxForm.Engine.Dtos;
using FauxForm.Engine.Models;
using FluentAssertions;
using Moq;
using FauxForm.Engine.Interfaces.Mapping;
using FluentValidation;

namespace FauxForm.UnitTests;
public class ContactFormServiceTest
{
    private readonly ContactFormService _contactFormService;
    private readonly Mock<IContactFormRepository> _mockContactFormRepository;
    private readonly Mock<IContactFormValidation> _mockValidator;
    private readonly Mock<IContactFormMapper> _mockMapper;

    public ContactFormServiceTest()
    {
        _mockContactFormRepository = new Mock<IContactFormRepository>();
        _mockValidator = new Mock<IContactFormValidation>();
        _mockMapper = new Mock<IContactFormMapper>();
        _contactFormService = new ContactFormService(_mockContactFormRepository.Object, _mockValidator.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Service_gets_contact_form()
    {
        _mockContactFormRepository.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ContactForm() { Name = "Bob"});
        _mockMapper.Setup(x => x.Map(It.IsAny<IContactFormModel>())).Returns(new ContactFormDto() { Name = "Bob" });

        var response = await _contactFormService.GetAsync("Bob", new CancellationToken());

        response.Should().NotBeNull();
        response.Name.Should().Be("Bob");
        _mockContactFormRepository.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Service_saves_contact_form()
    {
        await _contactFormService.SaveAsync(new ContactFormDto(), new CancellationToken());

        _mockContactFormRepository.Verify(x => x.SaveAsync(It.IsAny<IContactFormModel>(), It.IsAny<CancellationToken>()), Times.Once());
        _mockValidator.Verify(x => x.ValidateAndThrowAsync(It.IsAny<IContactForm>(), It.IsAny<CancellationToken>()), Times.Once());
    }


    [Fact]
    public async Task Service_doesnt_update_contact_form_throws_exception()
    {
        ContactForm model = null;
        _mockMapper.Setup(x => x.Map(It.IsAny<IContactFormDto>())).Returns(new ContactForm() { Name = "Bob" });
        _mockContactFormRepository.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);

        var act = () => _contactFormService.UpdateAsync(new ContactFormDto() { Name = "Bob" }, new CancellationToken());

        await act.Should().ThrowAsync<ValidationException>();
        _mockContactFormRepository.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        _mockContactFormRepository.Verify(x => x.SaveAsync(It.IsAny<IContactFormModel>(), It.IsAny<CancellationToken>()), Times.Never());
        _mockValidator.Verify(x => x.ValidateAndThrowAsync(It.IsAny<IContactForm>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Service_updates_contact_form()
    {
        _mockContactFormRepository.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ContactForm() { Name = "Bob" });

        await _contactFormService.UpdateAsync(new ContactFormDto() { Name = "Bob" }, new CancellationToken());

        _mockContactFormRepository.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        _mockContactFormRepository.Verify(x => x.SaveAsync(It.IsAny<IContactFormModel>(), It.IsAny<CancellationToken>()), Times.Once());
        _mockValidator.Verify(x => x.ValidateAndThrowAsync(It.IsAny<IContactForm>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Service_doesnt_delete_contact_form_throws_exception()
    {
        ContactForm model = null;
        _mockMapper.Setup(x => x.Map(It.IsAny<IContactFormDto>())).Returns(new ContactForm() { Name = "Bob" });
        _mockContactFormRepository.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);

        var act = () => _contactFormService.DeleteAsync(new ContactFormDto() { Name = "Bob" }, new CancellationToken());

        await act.Should().ThrowAsync<ValidationException>();
        _mockContactFormRepository.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        _mockContactFormRepository.Verify(x => x.DeleteAsync(It.IsAny<IContactFormModel>(), It.IsAny<CancellationToken>()), Times.Never());
        _mockValidator.Verify(x => x.ValidateAndThrowAsync(It.IsAny<IContactForm>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Service_deletes_contact_form()
    {
        _mockContactFormRepository.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ContactForm() { Name = "Bob" });

        await _contactFormService.DeleteAsync(new ContactFormDto() { Name = "Bob" }, new CancellationToken());

        _mockContactFormRepository.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        _mockContactFormRepository.Verify(x => x.DeleteAsync(It.IsAny<IContactFormModel>(), It.IsAny<CancellationToken>()), Times.Once());
        _mockValidator.Verify(x => x.ValidateAndThrowAsync(It.IsAny<IContactForm>(), It.IsAny<CancellationToken>()), Times.Once());
    }
}
