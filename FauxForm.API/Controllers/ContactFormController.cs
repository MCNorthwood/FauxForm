using FauxForm.Engine.Interfaces;
using FauxForm.Engine.Dtos;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace FauxForm.API.Controllers;
[ApiController]
[Route("[controller]")]
public class ContactFormController(IContactFormService contactFormService) : ControllerBase
{
    [HttpGet(Name = "GetContactForm")]
    public async Task<IActionResult> Index(string name)
    {
        try
        {
            var response = await contactFormService.GetAsync(name, new CancellationToken());
            return Ok(response);
        }
        catch (ValidationException vex)
        {
            return BadRequest(vex.Message);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: 500);
        }
    }

    [HttpPost(Name = "SaveContactForm")]
    public async Task<IActionResult> Save(ContactFormDto formDto)
    {
        try
        {
            await contactFormService.SaveAsync(formDto, new CancellationToken());
            return Created("/ContactForm?name=", formDto.Name);
        }
        catch(ValidationException vex)
        {
            return BadRequest(vex.Message);
        }
        catch(Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: 500);
        }
    }

    [HttpPut(Name = "UpdateContactForm")]
    public async Task<IActionResult> Update(ContactFormDto formDto)
    {
        try
        {
            await contactFormService.UpdateAsync(formDto, new CancellationToken());
            return Ok();
        }
        catch (ValidationException vex)
        {
            return BadRequest(vex.Message);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: 500);
        }
    }

    [HttpDelete(Name = "DeleteContactForm")]
    public async Task<IActionResult> Delete(ContactFormDto formDto)
    {
        try
        {
            await contactFormService.DeleteAsync(formDto, new CancellationToken());
            return Ok();
        }
        catch (ValidationException vex)
        {
            return BadRequest(vex.Message);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: 500);
        }
    }
}
