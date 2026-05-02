using Microsoft.EntityFrameworkCore;
using InternshipPortal.Data;

var builder = WebApplication.CreateBuilder(args);

// --- DATABASE CONNECTION LOGIC (FIXED FOR CLOUD) ---
// Pehle ye check karega ke kya hum Render par hain (DATABASE_URL mojood hai?)
// Agar nahi, toh appsettings.json wala local connection uthayega.
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
// ---------------------------------------------------

// 2. COOKIE AUTHENTICATION (This remembers who is logged in)
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "UserSession";
        options.LoginPath = "/Account/Login";   // Where to go if not logged in
        options.AccessDeniedPath = "/Account/Login"; // Where to go if role is wrong
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. SECURITY MIDDLEWARE (The order here is very important!)
app.UseAuthentication(); // This checks WHO you are (Identity)
app.UseAuthorization();  // This checks WHAT you can do (Roles)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();