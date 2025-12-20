using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CampusActivityHub.Models;

namespace CampusActivityHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Registration>()
                .HasKey(r => new { r.EventId, r.UserId });

            builder.Entity<Registration>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId);

            builder.Entity<Registration>()
                .HasOne(r => r.Student)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.UserId);

            builder.Entity<Event>().HasQueryFilter(e => !e.IsDeleted);

            builder.Entity<Event>()
                .HasMany(e => e.Tags)
                .WithMany(t => t.Events);
        }
    }
}