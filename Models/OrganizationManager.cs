namespace MostraCorporatePortal.Models;

public class OrganizationManager
{
    public int Id { get; set; }

    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    public string CollaboratorId { get; set; } = null!;
    public Collaborator Collaborator { get; set; } = null!;
}