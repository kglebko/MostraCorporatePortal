using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MostraCorporatePortal.Models;

public class Collaborator : IdentityUser
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }

    [NotMapped]
    public string FullName =>
        string.IsNullOrWhiteSpace(MiddleName)
            ? $"{LastName} {FirstName}"
            : $"{LastName} {FirstName} {MiddleName}";

    public DateTime BirthDate { get; set; }

    public int PositionId { get; set; }
    public Position? Position { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? WorkFormatId { get; set; }
    public WorkFormat? WorkFormat { get; set; }

    public int OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public int RoleId { get; set; }
    public Role? Role { get; set; }

    public string? MobilePhone { get; set; }
    public string? InternalPhone { get; set; }
    public string? Photo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}