using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.Models
{
    public class Internship
    {
        [Key]
        public int InternshipId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RequiredSkills { get; set; } = string.Empty; // Example: "C#,SQL,HTML"
        public string Location { get; set; } = string.Empty;
    }
}