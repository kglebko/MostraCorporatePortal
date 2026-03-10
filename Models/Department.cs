namespace MostraCorporatePortal.Models;

public class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    // родительское подразделение
    public int? ParentDepartmentId { get; set; }
    public Department? ParentDepartment { get; set; }

    // дочерние подразделения
    public ICollection<Department>? ChildDepartments { get; set; }

    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;

    public ICollection<Collaborator>? Collaborators { get; set; }

    public ICollection<DepartmentManager>? Managers { get; set; }
}