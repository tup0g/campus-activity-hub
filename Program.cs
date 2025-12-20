using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CampusActivityHub.Data;
using CampusActivityHub.Models;   // Виправляє помилку: 'User' could not be found
using CampusActivityHub.Services; // Виправляє помилку: Unable to resolve service 'IEmailService'
using CampusActivityHub.Filters;  // Для фільтрів помилок

var builder = WebApplication.CreateBuilder(args);

// 1. Підключення до Бази Даних
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// 2. Налаштування Identity
// ВАЖЛИВО: Ми використовуємо 'User', а не 'IdentityUser'.
// Це виправляє конфлікт типів, але вимагає зміни в _LoginPartial (див. нижче).
builder.Services.AddIdentity<User, IdentityRole>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4; 
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

// 3. Реєстрація сервісу Email (Виправляє помилку зі скріншота 16:15)
builder.Services.AddTransient<IEmailService, FileEmailService>();

// 4. Реєстрація контролерів та фільтрів
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter));
});

builder.Services.AddRazorPages();

var app = builder.Build();

// 5. Конфігурація
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 6. Маршрути
app.MapControllerRoute(
    name: "eventsByDate",
    pattern: "events/{year:int}/{month:int}",
    defaults: new { controller = "Events", action = "Archive" });

app.MapControllerRoute(
    name: "eventDetails",
    pattern: "event/{id:int}/{slug}",
    defaults: new { controller = "Events", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Events}/{action=Index}/{id?}");

app.MapRazorPages();

// 7. Ініціалізація даних (щоб база не була порожньою)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        await DbInitializer.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();