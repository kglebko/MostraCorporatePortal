namespace MostraCorporatePortal.DTO;

public class CollaboratorResponseDto
{
    public string Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string? Position { get; set; }
    public string? Department { get; set; }
    public string? WorkFormat { get; set; }
    public string? Organization { get; set; }
    public string? Role { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? MobilePhone { get; set; }
    public string? InternalPhone { get; set; }
    public DateTime CreatedAt { get; set; }
}