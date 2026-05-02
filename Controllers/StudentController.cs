using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InternshipPortal.Data;
using InternshipPortal.Models;
using System.Security.Claims;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace InternshipPortal.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public StudentController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // --- DASHBOARD & PROFILE LOGIC ---

        public IActionResult Index(string? searchString)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null) return RedirectToAction("Login", "Account");
            int userId = int.Parse(userIdClaim);

            var profile = _context.StudentProfiles.FirstOrDefault(p => p.UserId == userId);
            string studentSkills = profile?.Skills ?? "";

            var internships = _context.Internships.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                internships = internships.Where(s => s.Title.ToLower().Contains(searchString.ToLower()) || s.CompanyName.ToLower().Contains(searchString.ToLower()));
            }

            var jobMatches = internships.ToList().Select(job => new {
                Internship = job,
                Score = CalculateMatchScore(studentSkills, job.RequiredSkills)
            }).OrderByDescending(x => x.Score).ToList();

            ViewBag.Search = searchString;
            return View(jobMatches);
        }

        public IActionResult Profile()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var profile = _context.StudentProfiles.FirstOrDefault(p => p.UserId == userId);
            return View(profile ?? new StudentProfile { UserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> Profile(StudentProfile model, IFormFile? resumeFile)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var existingProfile = _context.StudentProfiles.FirstOrDefault(p => p.UserId == userId);

            if (resumeFile != null && resumeFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(resumeFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resumes");
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
                using (var stream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                {
                    await resumeFile.CopyToAsync(stream);
                }
                model.ResumePath = fileName;
            }

            if (existingProfile == null) { model.UserId = userId; _context.StudentProfiles.Add(model); }
            else { existingProfile.Skills = model.Skills; existingProfile.Education = model.Education; if (!string.IsNullOrEmpty(model.ResumePath)) existingProfile.ResumePath = model.ResumePath; }

            _context.SaveChanges();
            TempData["Success"] = "Profile Updated!";
            return RedirectToAction("Index");
        }

        // --- AI RESUME BOT LOGIC (OLLAMA INTEGRATION) ---

        public IActionResult ResumeBot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuildResumeAI([FromBody] ChatHistoryRequest request)
        {
            // Local Ollama API Endpoint
            var apiUrl = "http://localhost:11434/api/chat";
            using var client = new HttpClient();

            // 1. PROFESSIONAL SYSTEM PROMPT
            string systemPrompt = @"You are a Professional Resume Assistant. 
            Your goal is to collect Name, Email, Education, Skills, and Experience.
            Format the resume using clean HTML (h1, h3, ul, li). 
            You MUST wrap the generated resume HTML between [RESUME_START] and [RESUME_END] tags. 
            Example: [RESUME_START]<h1>John Doe</h1>[RESUME_END]";

            var messages = new List<object>();
            messages.Add(new { role = "system", content = systemPrompt });

            // 2. CONVERT HISTORY FOR OLLAMA
            if (!string.IsNullOrEmpty(request.History))
            {
                try
                {
                    var historyData = JsonSerializer.Deserialize<List<JsonElement>>(request.History);
                    foreach (var item in historyData)
                    {
                        string role = item.GetProperty("role").GetString() == "model" ? "assistant" : "user";
                        if (item.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                        {
                            string contentText = parts[0].GetProperty("text").GetString() ?? "";
                            messages.Add(new { role = role, content = contentText });
                        }
                    }
                }
                catch { }
            }

            // 3. ADD LATEST USER MESSAGE
            messages.Add(new { role = "user", content = request.Message ?? "" });

            var payload = new
            {
                model = "llama3", // This must match exactly what you just pulled in CMD
                messages = messages,
                stream = false
            };

            try
            {
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);

                // SAFE PARSING OF OLLAMA RESPONSE
                if (doc.RootElement.TryGetProperty("message", out var msgElement))
                {
                    var aiText = msgElement.GetProperty("content").GetString();
                    return Json(new { reply = aiText });
                }
                return Json(new { reply = "Ollama returned an empty response." });
            }
            catch (Exception ex)
            {
                return Json(new { reply = "Connection Error: Ensure Ollama is running. " + ex.Message });
            }
        }

        // --- HELPER METHODS ---

        [HttpPost]
        public IActionResult Apply(int internshipId, double matchScore)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return RedirectToAction("Login", "Account");
            int userId = int.Parse(userIdClaim);
            if (!_context.Applications.Any(a => a.InternshipId == internshipId && a.StudentId == userId))
            {
                _context.Applications.Add(new Application { InternshipId = internshipId, StudentId = userId, MatchScore = matchScore, Status = "Pending" });
                _context.SaveChanges();
                TempData["Success"] = "Applied Successfully!";
            }
            return RedirectToAction("Index");
        }

        public IActionResult MyApplications()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var myApps = (from app in _context.Applications
                          join job in _context.Internships on app.InternshipId equals job.InternshipId
                          where app.StudentId == userId
                          select new { JobTitle = job.Title, CompanyName = job.CompanyName, Score = app.MatchScore, Status = app.Status }).ToList();
            return View(myApps);
        }

        private double CalculateMatchScore(string studentSkills, string jobSkills)
        {
            if (string.IsNullOrEmpty(studentSkills) || string.IsNullOrEmpty(jobSkills)) return 0;
            var sSkills = studentSkills.ToLower().Split(',').Select(s => s.Trim()).ToList();
            var jSkills = jobSkills.ToLower().Split(',').Select(s => s.Trim()).ToList();
            int matches = sSkills.Intersect(jSkills).Count();
            return Math.Round((double)matches / jSkills.Count * 100, 0);
        }
    }

    public class ChatHistoryRequest
    {
        public string? History { get; set; }
        public string? Message { get; set; }
    }
}