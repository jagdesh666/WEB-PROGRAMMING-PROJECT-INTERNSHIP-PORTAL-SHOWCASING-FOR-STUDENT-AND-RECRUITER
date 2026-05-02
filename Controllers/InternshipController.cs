using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InternshipPortal.Data;
using InternshipPortal.Models;

namespace InternshipPortal.Controllers
{
    [Authorize] // You must be logged in to see this
    public class InternshipController : Controller
    {
        private readonly ApplicationDbContext _context;
        public InternshipController(ApplicationDbContext context) { _context = context; }

        // List all internships
        public IActionResult Index()
        {
            var list = _context.Internships.ToList();
            return View(list);
        }

        // GET: Create Page (Only for Recruiters)
        [Authorize(Roles = "Recruiter")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Save Internship
        [HttpPost]
        [Authorize(Roles = "Recruiter")]
        public IActionResult Create(Internship internship)
        {
            if (ModelState.IsValid)
            {
                _context.Internships.Add(internship);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(internship);
        }


        [Authorize(Roles = "Recruiter")]
        public IActionResult Applicants()
        {
            var applicants = (from app in _context.Applications
                              join job in _context.Internships on app.InternshipId equals job.InternshipId
                              join user in _context.Users on app.StudentId equals user.UserId
                              // We join StudentProfiles to get the ResumePath
                              join profile in _context.StudentProfiles on user.UserId equals profile.UserId into profileJoin
                              from p in profileJoin.DefaultIfEmpty()
                              select new
                              {
                                  ApplicationId = app.ApplicationId,
                                  StudentName = user.FullName,
                                  JobTitle = job.Title,
                                  Score = app.MatchScore,
                                  Status = app.Status,
                                  ResumePath = p != null ? p.ResumePath : "" // Get the Resume Path here
                              }).ToList();

            return View(applicants);
        }

        // ALSO ADD THIS METHOD if you haven't yet:
        [HttpPost]
        public IActionResult UpdateStatus(int appId, string status)
        {
            var app = _context.Applications.Find(appId);
            if (app != null)
            {
                app.Status = status;
                _context.SaveChanges();
            }
            return RedirectToAction("Applicants");
        }

        // GET: Internship/Edit/5
        [Authorize(Roles = "Recruiter")]
        public IActionResult Edit(int id)
        {
            var job = _context.Internships.Find(id);
            return View(job);
        }

        [HttpPost]
        [Authorize(Roles = "Recruiter")]
        public IActionResult Edit(Internship job)
        {
            _context.Internships.Update(job);
            _context.SaveChanges();
            TempData["Success"] = "Internship updated successfully!";
            return RedirectToAction("Index");
        }

        // POST: Internship/Delete/5
        [HttpPost]
        [Authorize(Roles = "Recruiter")]
        public IActionResult Delete(int id)
        {
            var job = _context.Internships.Find(id);
            if (job != null)
            {
                _context.Internships.Remove(job);
                _context.SaveChanges();
                TempData["Success"] = "Internship deleted!";
            }
            return RedirectToAction("Index");
        }
    }
}