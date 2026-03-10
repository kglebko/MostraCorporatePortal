using System;

namespace MostraCorporatePortal.DTO;

public class CollaboratorDto
{
    public string Id { get; set; } = default!;
    public string FullName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Position { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? WorkFormat { get; set; }
    
    public int DepartmentId { get; set; }
    
    public int OrganizationId { get; set; }
    public string Organization { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? MobilePhone { get; set; }
    public string? InternalPhone { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? Photo { get; set; }
}