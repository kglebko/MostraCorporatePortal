namespace MostraCorporatePortal.Dto;

public class NewsDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Image { get; set; }

    public DateTime Date { get; set; }
}