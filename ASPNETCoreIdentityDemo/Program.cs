using ASPNETCoreIdentityDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure the Application Cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    // If the LoginPath isn't set, ASP.NET Core defaults the path to /Account/Login.
    options.LoginPath = "/Account/Login"; // Set your login path here
});

//Configuration Identity Services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options =>
    {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredUniqueChars = 4;

        // Other settings can be configured here
        //https://dotnettutorials.net/lesson/custom-password-policy-in-asp-net-core-identity/
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

var connectionString = builder.Configuration.GetConnectionString("SQLServerIdentityConnection") 
    ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
