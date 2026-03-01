using CorporatePortal.AuthServer.Data;
using CorporatePortal.AuthServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CorporatePortal.AuthServer;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext context, UserManager<Collaborator> userManager, RoleManager<IdentityRole> roleManager)
    {
        await context.Database.EnsureCreatedAsync();
        
        if (!await roleManager.RoleExistsAsync("Ad  min"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
        
        var testUser = await userManager.FindByNameAsync("k_glebko");
        if (testUser == null)
        {
            var position = await context.Positions.FindAsync(1);
            var department = await context.Departments.FindAsync(1);
            var organization = await context.Organizations.FindAsync(1);
            var role = await context.Roles.FindAsync(1);
            var workFormat = await context.WorkFormats.FindAsync(1);
            
            if (position == null)
            {
                position = new Position { Id = 1, Name = "Должность" };
                context.Positions.Add(position);
                await context.SaveChangesAsync();
            }

            if (department == null)
            {
                department = new Department { Id = 1, Name = "Департамент" };
                context.Departments.Add(department);
                await context.SaveChangesAsync();
            }

            if (organization == null)
            {
                organization = new Organization { Id = 1, Name = "Организация" };
                context.Organizations.Add(organization);
                await context.SaveChangesAsync();
            }

            if (role == null)
            {
                role = new Role { Id = 1, Name = "Роль" };
                context.Roles.Add(role);
                await context.SaveChangesAsync();
            }

            if (workFormat == null)
            {
                workFormat = new WorkFormat { Id = 1, Name = "Формат работы" };
                context.WorkFormats.Add(workFormat);
                await context.SaveChangesAsync();
            }

            testUser = new Collaborator
            {
                UserName = "k_glebko",
                Email = "k_glebko@mostra.by",
                FirstName = "Константин",
                LastName = "Глебко",
                MiddleName = "Александрович",
                BirthDate = new DateTime(1977, 12, 2),
                PositionId = 1,
                DepartmentId = 1,
                OrganizationId = 1,
                RoleId = 1,
                WorkFormatId = 1,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(testUser, "Qwerty123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(testUser, "User");
                Console.WriteLine("Пользователь k_glebko успешно создан!");
            }
            else
            {
                Console.WriteLine("Ошибки при создании пользователя:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine("Пользователь k_glebko уже существует.");
        }
    }
}

