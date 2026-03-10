using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MostraCorporatePortal.Data;
using MostraCorporatePortal.Models;

[ApiController]
[Route("api/departments")]
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartmentsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartment(int id)
    {
        var department = await _context.Departments
            .Include(d => d.Organization)
            .Include(d => d.ParentDepartment)
            .Include(d => d.Managers)
            .ThenInclude(m => m.Collaborator)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
            return NotFound();

        return Ok(new
        {
            id = department.Id,
            name = department.Name,
            organizationId = department.OrganizationId,
            organizationName = department.Organization.Name,

            parentDepartmentId = department.ParentDepartmentId,
            parentDepartmentName = department.ParentDepartment != null
                ? department.ParentDepartment.Name
                : null,

            managers = department.Managers.Select(m => new
            {
                id = m.Collaborator.Id,
                fullName = m.Collaborator.LastName + " " +
                           m.Collaborator.FirstName + " " +
                           m.Collaborator.MiddleName
            })
        });
    }
    
    [HttpGet("{id}/structure")]
    public async Task<IActionResult> GetStructure(int id)
    {
        var departments = await _context.Departments
            .Where(d => d.OrganizationId == id)
            .ToListAsync();

        var tree = departments
            .Where(d => d.ParentDepartmentId == null)
            .Select(d => BuildTree(d, departments));

        return Ok(tree);
    }

    private object BuildTree(Department department, List<Department> all)
    {
        return new
        {
            id = department.Id,
            name = department.Name,
            children = all
                .Where(d => d.ParentDepartmentId == department.Id)
                .Select(d => BuildTree(d, all))
        };
    }
}