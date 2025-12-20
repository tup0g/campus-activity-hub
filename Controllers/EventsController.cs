using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusActivityHub.Data;
using CampusActivityHub.Models;
using CampusActivityHub.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CampusActivityHub.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IEmailService _emailService;

        public EventsController(ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment appEnvironment, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Registrations)
                .Include(e => e.Tags)
                .OrderBy(e => e.Date)
                .ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Archive(int year, int month)
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .Where(e => e.Date.Year == year && e.Date.Month == month)
                .ToListAsync();
                
            ViewBag.FilterName = $"Архів за {month}/{year}";
            return View("Index", events);
        }

        public async Task<IActionResult> Details(int id, string slug)
        {
            var eventItem = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Tags)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null) return NotFound();
            return View(eventItem);
        }

        [Authorize(Roles = "Admin, Organizer")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Organizer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model, IFormFile? imageFile, string TagsString) // TagsString з форми
        {
            ModelState.Remove("Organizer");
            ModelState.Remove("OrganizerId");
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                model.OrganizerId = user.Id;

                if (imageFile != null)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    string path = Path.Combine(_appEnvironment.WebRootPath, "images", fileName);
                    using (var stream = new FileStream(path, FileMode.Create)) await imageFile.CopyToAsync(stream);
                    model.ImagePath = fileName;
                }

                if (!string.IsNullOrEmpty(TagsString))
                {
                    var tagNames = TagsString.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim());
                    foreach (var tagName in tagNames)
                    {
                        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName) 
                                  ?? new Tag { Name = tagName };
                        model.Tags.Add(tag);
                    }
                }

                _context.Add(model);
                await _context.SaveChangesAsync();

                await _emailService.SendEmailAsync(user.Email, "Подію створено!", $"Ви створили подію: {model.Title}");

                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(model);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> JoinAjax(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);
            var eventItem = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventItem == null) return Json(new { success = false, message = "Подію не знайдено" });
            
            if (eventItem.Registrations.Any(r => r.UserId == user.Id))
                return Json(new { success = false, message = "Ви вже зареєстровані" });

            if (eventItem.Registrations.Count >= eventItem.MaxParticipants)
                return Json(new { success = false, message = "Місць немає" });

            _context.Registrations.Add(new Registration { EventId = eventId, UserId = user.Id });
            await _context.SaveChangesAsync();
            
            try {
                await _emailService.SendEmailAsync(user.Email, "Реєстрація успішна", $"Ви йдете на {eventItem.Title}");
            } catch {}

            int currentCount = await _context.Registrations.CountAsync(r => r.EventId == eventId);
            int placesLeft = eventItem.MaxParticipants - currentCount;

            return Json(new { success = true, placesLeft = placesLeft });
        }
    }
}