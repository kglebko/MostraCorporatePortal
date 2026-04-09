using System.ComponentModel.DataAnnotations;

namespace MostraCorporatePortal.Models;

public class News
{
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? Image { get; set; }

    public DateTime Date { get; set; }

    public DateTime? DatePost { get; set; }

    public bool Visible { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}