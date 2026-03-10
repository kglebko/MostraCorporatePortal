namespace MostraCorporatePortal.Models;

public class DepartmentManager
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public string CollaboratorId { get; set; } = null!;
    public Collaborator Collaborator { get; set; } = null!;
}