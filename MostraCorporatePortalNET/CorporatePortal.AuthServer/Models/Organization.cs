namespace CorporatePortal.AuthServer.Models;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Collaborator>? Collaborators { get; set; }
}

