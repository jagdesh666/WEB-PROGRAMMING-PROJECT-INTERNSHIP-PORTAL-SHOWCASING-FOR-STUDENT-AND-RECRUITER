using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        public int InternshipId { get; set; }
        public int StudentId { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected
        public double MatchScore { get; set; } // This is where our AI logic result will stay
    }
}