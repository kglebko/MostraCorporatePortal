using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MostraCorporatePortal.Data;
using MostraCorporatePortal.Dto;

namespace MostraCorporatePortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PositionsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PositionDto>>> GetAll()
    {
        var positions = await _context.Positions
            .OrderBy(p => p.Name)
            .Select(p => new PositionDto
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync();

        return Ok(positions);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<PositionDto>> GetById(int id)
    {
        var position = await _context.Positions
            .Where(p => p.Id == id)
            .Select(p => new PositionDto
            {
                Id = p.Id,
                Name = p.Name
            })
            .FirstOrDefaultAsync();

        if (position == null)
            return NotFound();

        return Ok(position);
    }
}