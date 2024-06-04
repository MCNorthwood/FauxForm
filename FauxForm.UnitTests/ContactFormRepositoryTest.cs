using FauxForm.Engine.Repository;
using FauxForm.Engine.Models;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using System.Text;

namespace FauxForm.UnitTests;

public class ContactFormRepositoryTest
{
    private readonly ContactFormRepository _contactFormRepository;
    private readonly Mock<IDistributedCache> _mockCache;

    public ContactFormRepositoryTest()
    {
        _mockCache = new Mock<IDistributedCache>();
        _contactFormRepository = new ContactFormRepository(_mockCache.Object);
    }

    [Fact]
    public async Task Does_contact_form_save()
    {
        await _contactFormRepository.SaveAsync(new ContactForm(), new CancellationToken());

        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Does_contact_form_get()
    {
        var byteForm = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new ContactForm { Name = "Bob" })) ?? null;

        _mockCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(byteForm);

        var contactForm = await _contactFormRepository.GetAsync("Bob", new CancellationToken());

        contactForm.Should().NotBeNull();
        contactForm.Name.Should().Be("Bob");
        _mockCache.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Does_contact_form_delete()
    {
        _mockCache.Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        await _contactFormRepository.DeleteAsync(new ContactForm(), new CancellationToken());

        _mockCache.Verify(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }
}