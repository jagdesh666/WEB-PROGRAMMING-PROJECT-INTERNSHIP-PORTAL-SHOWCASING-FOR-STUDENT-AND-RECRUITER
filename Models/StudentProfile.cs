using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipPortal.Models
{
    public class StudentProfile
    {
        [Key]
        public int ProfileId { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public string Skills { get; set; } = string.Empty; // Example: "C#,HTML"
        public string Education { get; set; } = string.Empty;

        public string? ResumePath { get; set; }
    }
}