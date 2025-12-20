using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CampusActivityHub.Models;

namespace CampusActivityHub.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            if (context.Events.Any()) return;

            string[] roles = new[] { "Admin", "Organizer", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@test.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User { UserName = adminEmail, Email = adminEmail, FullName = "Головний Адмін", EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            var categories = new Category[]
            {
                new Category { Name = "Спорт", IconClass = "bi-bicycle" },
                new Category { Name = "IT та Наука", IconClass = "bi-laptop" },
                new Category { Name = "Вечірки", IconClass = "bi-music-note" },
                new Category { Name = "Мистецтво", IconClass = "bi-palette" }
            };

            foreach (var c in categories)
            {
                context.Categories.Add(c);
            }
            await context.SaveChangesAsync();

            var events = new Event[]
            {
                new Event
                {
                    Title = "Хакатон C# .NET",
                    Description = "Змагання для програмістів. Створюємо веб-додатки за 24 години.",
                    Date = DateTime.Now.AddDays(5).AddHours(10),
                    MaxParticipants = 50,
                    ImagePath = "hackathon.jpg",
                    Category = categories[1],
                    OrganizerId = adminUser.Id,
                    IsDeleted = false
                },
                new Event
                {
                    Title = "Футбольний матч",
                    Description = "Граємо факультет на факультет. Збір на стадіоні.",
                    Date = DateTime.Now.AddDays(2).AddHours(15),
                    MaxParticipants = 22,
                    ImagePath = "football.jpg",
                    Category = categories[0],
                    OrganizerId = adminUser.Id,
                    IsDeleted = false
                },
                new Event
                {
                    Title = "Вечір кіно",
                    Description = "Дивимось класику світового кіно просто неба.",
                    Date = DateTime.Now.AddDays(1).AddHours(19),
                    MaxParticipants = 100,
                    ImagePath = "cinema.jpg",
                    Category = categories[3], 
                    OrganizerId = adminUser.Id,
                    IsDeleted = false
                }
            };

            context.Events.AddRange(events);
            await context.SaveChangesAsync();
        }
    }
}