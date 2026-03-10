using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MostraCorporatePortal.Data;

[ApiController]
[Route("api/organizations")]
public class OrganizationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrganizationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrganization(int id)
    {
        var organization = await _context.Organizations
            .Include(o => o.Managers)
            .ThenInclude(m => m.Collaborator)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (organization == null)
            return NotFound();

        return Ok(new
        {
            id = organization.Id,
            name = organization.Name,

            managers = organization.Managers.Select(m => new {
                id = m.Collaborator.Id,
                fullName =
                    m.Collaborator.LastName + " " +
                    m.Collaborator.FirstName + " " +
                    m.Collaborator.MiddleName
            })
        });
    }
}