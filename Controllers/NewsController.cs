using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MostraCorporatePortal.Data;
using MostraCorporatePortal.Dto;

namespace MostraCorporatePortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly AppDbContext _context;

    public NewsController(AppDbContext context)
    {
        _context = context;
    }

    // Все новости
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NewsDto>>> GetAll()
    {
        var news = await _context.News
            .Where(n =>
                n.Visible &&
                (n.DatePost == null || n.DatePost <= DateTime.UtcNow)
            )
            .OrderByDescending(n => n.DatePost ?? n.Date)
            .Select(n => new NewsDto
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                Image = n.Image,
                Date = n.Date
            })
            .ToListAsync();

        return Ok(news);
    }

    // Последние N новостей
    [HttpGet("latest/{count}")]
    public async Task<ActionResult<IEnumerable<NewsDto>>> GetLatest(int count)
    {
        var news = await _context.News
            .Where(n =>
                n.Visible &&
                (n.DatePost == null || n.DatePost <= DateTime.UtcNow)
            )
            .OrderByDescending(n => n.DatePost ?? n.Date)
            .Take(count)
            .Select(n => new NewsDto
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                Image = n.Image,
                Date = n.Date
            })
            .ToListAsync();

        return Ok(news);
    }

    // Одна новость
    [HttpGet("{id}")]
    public async Task<ActionResult<NewsDto>> GetById(int id)
    {
        var news = await _context.News
            .Where(n =>
                n.Id == id &&
                n.Visible &&
                (n.DatePost == null || n.DatePost <= DateTime.UtcNow)
            )
            .Select(n => new NewsDto
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                Image = n.Image,
                Date = n.Date
            })
            .FirstOrDefaultAsync();

        if (news == null)
            return NotFound();

        return Ok(news);
    }
}