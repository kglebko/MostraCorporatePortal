namespace MostraCorporatePortal.Models;

public class WorkFormat
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Collaborator>? Collaborators { get; set; }
}