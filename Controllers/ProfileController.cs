using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MostraCorporatePortal.Data;
using MostraCorporatePortal.DTO;

namespace MostraCorporatePortal.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var user = await _context.Collaborators
            .Include(c => c.Position)
            .Include(c => c.Department)
            .Include(c => c.WorkFormat)
            .Include(c => c.Organization)
            .Include(c => c.Role)
            .Select(c => new CollaboratorDto
            {
                Id = c.Id,
                FullName = c.FullName,
                BirthDate = c.BirthDate,
                Position = c.Position!.Name,
                Department = c.Department!.Name,
                WorkFormat = c.WorkFormat != null ? c.WorkFormat.Name : null,
                Organization = c.Organization!.Name,
                Role = c.Role!.Name,
                Username = c.UserName,
                Email = c.Email,
                MobilePhone = c.MobilePhone,
                InternalPhone = c.InternalPhone,
                CreatedAt = c.CreatedAt,
                Photo = c.Photo
            })
            .FirstOrDefaultAsync(c => c.Id == userId);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}