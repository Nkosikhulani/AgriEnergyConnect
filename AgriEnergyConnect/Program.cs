using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data; // Ensure this is the correct namespace for ApplicationDbContext
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnect.Models; // Add this if you have a custom User class

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DevelopmentDB"); // Or "ProductionDB"

// 2. Register the ApplicationDbContext with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3.  If using Identity, configure Identity services
builder.Services.AddIdentity<User, IdentityRole>() // If using a custom User class
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// If you're using the default IdentityUser, use this instead:
// builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>()
//     .AddDefaultTokenProviders();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    // options.User.RequireConfirmedEmail = false; // Commenting out this line
    // options.User.RequireConfirmedPhoneNumber = false; // Already commented out
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();