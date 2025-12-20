using Microsoft.AspNetCore.Identity;

namespace CampusActivityHub.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName { get; set; } = string.Empty;
        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}