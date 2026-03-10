namespace MostraCorporatePortal.Models;

public class Organization
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Department>? Departments { get; set; }

    public ICollection<Collaborator>? Collaborators { get; set; }

    public ICollection<OrganizationManager>? Managers { get; set; }
}