using FauxForm.Engine.Interfaces.Models;

namespace FauxForm.Engine.Dtos;

public class ContactFormDto : IContactFormDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Enquiry { get; set; }
}
