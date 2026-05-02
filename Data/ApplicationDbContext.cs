using Microsoft.EntityFrameworkCore;
using InternshipPortal.Models;

namespace InternshipPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
}