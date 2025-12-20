using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusActivityHub.Data;
using CampusActivityHub.Models;
using CampusActivityHub.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CampusActivityHub.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var categoriesData = await _context.Events
                .Include(e => e.Category)
                .Where(e => e.Category != null)
                .GroupBy(e => e.Category.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToListAsync();

            var topEvents = await _context.Events
                .Include(e => e.Registrations)
                .OrderByDescending(e => e.Registrations.Count)
                .Take(5)
                .Select(e => new { Title = e.Title, Count = e.Registrations.Count })
                .ToListAsync();

            var model = new DashboardViewModel
            {
                CategoryLabels = categoriesData.Select(x => x.Name).ToList(),
                CategoryCounts = categoriesData.Select(x => x.Count).ToList(),
                EventLabels = topEvents.Select(x => x.Title).ToList(),
                EventParticipantsCounts = topEvents.Select(x => x.Count).ToList()
            };

            return View(model);
        }
    }
}