using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MostraCorporatePortal.Data;
using MostraCorporatePortal.Dto;
using MostraCorporatePortal.Models;
using Microsoft.AspNetCore.Identity;

namespace MostraCorporatePortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollaboratorsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<Collaborator> _passwordHasher;

    public CollaboratorsController(AppDbContext context, IPasswordHasher<Collaborator> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var collaborators = await _context.Collaborators
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
            .ToListAsync();

        return Ok(collaborators);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var collaborator = await _context.Collaborators
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
            .FirstOrDefaultAsync(c => c.Id == id);

        if (collaborator == null)
            return NotFound();

        return Ok(collaborator);
    }

    [HttpPost]
public async Task<IActionResult> Create([FromBody] CreateCollaboratorDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var collaborator = new Collaborator
    {
        LastName = dto.LastName,
        FirstName = dto.FirstName,
        MiddleName = dto.MiddleName,
        BirthDate = dto.BirthDate,
        PositionId = dto.PositionId,
        DepartmentId = dto.DepartmentId,
        WorkFormatId = dto.WorkFormatId,
        OrganizationId = dto.OrganizationId,
        RoleId = dto.RoleId,
        UserName = dto.Username,
        NormalizedUserName = dto.Username.ToUpper(),
        Email = dto.Email,
        NormalizedEmail = dto.Email.ToUpper(),
        MobilePhone = dto.MobilePhone,
        InternalPhone = dto.InternalPhone,
        CreatedAt = DateTime.UtcNow,
        Photo = dto.Photo
    };
    
    collaborator.PasswordHash = _passwordHasher.HashPassword(collaborator, dto.Password);

    _context.Collaborators.Add(collaborator);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(Get),
        new { id = collaborator.Id },
        new CollaboratorDto
        {
            Id = collaborator.Id,
            FullName = collaborator.FullName,
            BirthDate = collaborator.BirthDate,
            Position = (await _context.Positions.FindAsync(collaborator.PositionId))!.Name,
            Department = (await _context.Departments.FindAsync(collaborator.DepartmentId))!.Name,
            WorkFormat = collaborator.WorkFormatId != null
                ? (await _context.WorkFormats.FindAsync(collaborator.WorkFormatId))?.Name
                : null,
            Organization = (await _context.Organizations.FindAsync(collaborator.OrganizationId))!.Name,
            Role = (await _context.Roles.FindAsync(collaborator.RoleId))!.Name,
            Username = collaborator.UserName,
            Email = collaborator.Email,
            MobilePhone = collaborator.MobilePhone,
            InternalPhone = collaborator.InternalPhone,
            CreatedAt = collaborator.CreatedAt
        });
    }
}