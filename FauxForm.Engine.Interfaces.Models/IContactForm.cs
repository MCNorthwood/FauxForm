namespace FauxForm.Engine.Interfaces.Models;
public interface IContactForm
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Enquiry { get; set; }
}
