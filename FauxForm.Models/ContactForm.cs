using FauxForm.Engine.Interfaces.Models;

namespace FauxForm.Engine.Models;

public class ContactForm : IContactFormModel
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Enquiry { get; set; }
}
