using System;

namespace CampusActivityHub.Models
{
    public class Registration
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public string UserId { get; set; }
        public User Student { get; set; } = null!;

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}