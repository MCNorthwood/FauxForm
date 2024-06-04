using Autofac;
using FauxForm.Engine.Interfaces;
using FauxForm.Engine.Dtos;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using FauxForm.Engine.Models;

namespace FauxForm.ComponentTests;

public class ContactFormComponentTest
{
    private readonly IContactFormService _contactFormService;
    private readonly Mock<IDistributedCache> _mockCache;
    private readonly ContactFormDto _form;

    public ContactFormComponentTest()
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterAssemblyTypes(Assembly.Load("FauxForm.Engine.Repository")).AsImplementedInterfaces();
        containerBuilder.RegisterAssemblyTypes(Assembly.Load("FauxForm.Engine.Services")).AsImplementedInterfaces();
        containerBuilder.RegisterAssemblyTypes(Assembly.Load("FauxForm.Engine.Mapping")).AsImplementedInterfaces();
        containerBuilder.RegisterAssemblyTypes(Assembly.Load("FauxForm.Engine.Validation")).AsImplementedInterfaces();
        _mockCache = new Mock<IDistributedCache>();
        containerBuilder.RegisterInstance(_mockCache.Object);
        var container = containerBuilder.Build();

        _contactFormService = container.Resolve<IContactFormService>();
        _form = new ContactFormDto()
        {
            Name = "Test",
            Email = "Test@Test.com",
            Phone = "01915555555",
            Enquiry = "This is a test"
        };
    }

    [Fact]
    public async Task Does_contact_form_service_get_form()
    {
        var byteForm = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_form)) ?? null;

        _mockCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(byteForm);

        var response = await _contactFormService.GetAsync("Test", new CancellationToken());

        response.Should().NotBeNull();
        response.Name.Should().Be("Test");
        response.Email.Should().Be("Test@Test.com");
        response.Phone.Should().Be("01915555555");
        response.Enquiry.Should().Be("This is a test");
        _mockCache.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Does_contact_form_service_save_form()
    {
        await _contactFormService.SaveAsync(_form, new CancellationToken());

        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Does_contact_form_service_save_form_throw_validation_exception()
    {
        var act = () => _contactFormService.SaveAsync(new ContactFormDto(), new CancellationToken());

        await act.Should().ThrowAsync<ValidationException>();

        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Does_contact_form_service_update_form()
    {
        var byteForm = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_form)) ?? null;

        _mockCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(byteForm);

        await _contactFormService.UpdateAsync(_form, new CancellationToken());

        _mockCache.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Does_contact_form_service_update_form_throw_validation_exception()
    {
        var act = () => _contactFormService.UpdateAsync(new ContactFormDto(), new CancellationToken());

        await act.Should().ThrowAsync<ValidationException>();

        _mockCache.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Does_contact_form_service_delete_form()
    {
        var byteForm = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_form)) ?? null;

        _mockCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(byteForm);

        await _contactFormService.DeleteAsync(_form, new CancellationToken());

        _mockCache.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockCache.Verify(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Does_contact_form_service_delete_form_throw_validation_exception()
    {
        var act = () => _contactFormService.DeleteAsync(new ContactFormDto(), new CancellationToken());

        await act.Should().ThrowAsync<ValidationException>();

        _mockCache.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockCache.Verify(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}