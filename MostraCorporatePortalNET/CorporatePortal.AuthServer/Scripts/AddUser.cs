using CorporatePortal.AuthServer.Data;
using CorporatePortal.AuthServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CorporatePortal.AuthServer.Scripts;

public static class AddUserScript
{
    public static async Task AddUserAsync(ApplicationDbContext context, UserManager<Collaborator> userManager)
    {
        var existingUser = await userManager.FindByNameAsync("k_glebko");
        if (existingUser != null)
        {
            Console.WriteLine("Пользователь k_glebko уже существует!");
            return;
        }
        
        var position = await context.Positions.FindAsync(1);
        var department = await context.Departments.FindAsync(1);
        var organization = await context.Organizations.FindAsync(1);
        var role = await context.Roles.FindAsync(1);
        var workFormat = await context.WorkFormats.FindAsync(1);

        if (position == null || department == null || organization == null || role == null || workFormat == null)
        {
            Console.WriteLine("Ошибка: Не все необходимые записи найдены в базе данных!");
            Console.WriteLine($"Position (ID=1): {(position != null ? "найдена" : "НЕ найдена")}");
            Console.WriteLine($"Department (ID=1): {(department != null ? "найден" : "НЕ найден")}");
            Console.WriteLine($"Organization (ID=1): {(organization != null ? "найдена" : "НЕ найдена")}");
            Console.WriteLine($"Role (ID=1): {(role != null ? "найдена" : "НЕ найдена")}");
            Console.WriteLine($"WorkFormat (ID=1): {(workFormat != null ? "найден" : "НЕ найден")}");
            return;
        }

        var newUser = new Collaborator
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

        var result = await userManager.CreateAsync(newUser, "Qwerty123!");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "User");
            Console.WriteLine("✓ Пользователь k_glebko успешно создан!");
            Console.WriteLine($"  Email: {newUser.Email}");
            Console.WriteLine($"  ФИО: {newUser.FullName}");
            Console.WriteLine($"  Дата рождения: {newUser.BirthDate:yyyy-MM-dd}");
        }
        else
        {
            Console.WriteLine("✗ Ошибки при создании пользователя:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  - {error.Description}");
            }
        }
    }
}

