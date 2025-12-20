using System.ComponentModel.DataAnnotations;
namespace CampusActivityHub.Models;
public class Event
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    
    public DateTime Date { get; set; }
    public int MaxParticipants { get; set; }
    public bool IsDeleted { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public string OrganizerId { get; set; }
    public User Organizer { get; set; }

    public ICollection<Registration> Registrations { get; set; }

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}