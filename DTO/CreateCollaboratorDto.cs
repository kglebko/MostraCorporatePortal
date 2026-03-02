using System;
using System.ComponentModel.DataAnnotations;

namespace MostraCorporatePortal.DTO;

public class CreateCollaboratorDto
{
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    [Required] public DateTime BirthDate { get; set; }
    [Required] public int PositionId { get; set; }
    [Required] public int DepartmentId { get; set; }
    public int? WorkFormatId { get; set; }
    [Required] public int OrganizationId { get; set; }
    [Required] public int RoleId { get; set; }

    [Required] [MaxLength(50)] public string Username { get; set; } = string.Empty;
    [Required] [MaxLength(50)] public string Password { get; set; } = string.Empty;
    [Required] [MaxLength(100)] public string Email { get; set; } = string.Empty;

    public string? MobilePhone { get; set; }
    public string? InternalPhone { get; set; }
    public string? Photo { get; set; }
}