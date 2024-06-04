using FauxForm.Engine.Dtos;
using FauxForm.Engine.Interfaces.Models;
using FauxForm.Engine.Mapping;
using FauxForm.Engine.Models;

namespace FauxForm.UnitTests;
public class ContactFormMappingTest
{
    private readonly FormMapper _mapper;

    public ContactFormMappingTest()
    {
        _mapper = new FormMapper();
    }

    [Fact]
    public void Mapper_maps_model_to_dbo()
    {
        var response = _mapper.Map(new ContactForm());

        Assert.NotNull(response);
    }

    [Fact]
    public void Mapper_maps_dbo_to_model()
    {
        var response = _mapper.Map(new ContactFormDto());

        Assert.NotNull(response);
    }


    [Fact]
    public void Mapper_maps_null_to_dbo()
    {
        ContactForm model = null;
        var response = _mapper.Map(model);

        Assert.NotNull(response);
    }

    [Fact]
    public void Mapper_maps_null_to_model()
    {
        ContactFormDto model = null;
        var response = _mapper.Map(model);

        Assert.NotNull(response);
    }
}
