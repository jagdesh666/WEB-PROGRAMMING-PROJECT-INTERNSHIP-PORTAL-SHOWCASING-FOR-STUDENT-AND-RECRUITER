using Microsoft.AspNetCore.Mvc;
using InternshipPortal.Data;
using InternshipPortal.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BCrypt.Net; // Added for password hashing

namespace InternshipPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Register Page
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
[HttpPost]
public IActionResult Register(User user)
{
    if (ModelState.IsValid)
    {
        // 1. CHECK IF EMAIL IS ALREADY IN THE DATABASE
        // This looks for the email regardless of the role (Student or Recruiter)
        var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

        if (existingUser != null)
        {
            // 2. SHOW THE SPECIFIC MESSAGE YOU REQUESTED
            // If the user exists, we stop here and show the error.
            ViewBag.Error = "This email is already registered. Please use another one or login.";
            return View(user);
        }

        // 3. If email is unique, proceed with security and saving
        // Hash the password
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        // Save to PostgreSQL
        _context.Users.Add(user);
        _context.SaveChanges();

        TempData["Success"] = "Account created successfully!";
        return RedirectToAction("Login");
    }
    
    return View(user);
}

        // GET: Login Page
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // 1. Find the user by Email only
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            // 2. Verify the hashed password
            // BCrypt.Verify takes the "Plain Text" password from the user and compares it to the "Hash" in the DB
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserId", user.UserId.ToString())
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                if (user.Role == "Recruiter")
                {
                    return RedirectToAction("Index", "Internship");
                }
                else if (user.Role == "Student")
                {
                    return RedirectToAction("Index", "Student");
                }
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }

        // Logout logic
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}