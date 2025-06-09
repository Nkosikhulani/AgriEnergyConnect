// USING STATEMENTS MUST BE AT THE VERY TOP
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data; // Ensure this is correct for ApplicationDbContext
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnect.Models; // Ensure this is correct for your custom User class
using AgriEnergyConnect.Data.Seeds; // Add this using for the RoleSeeder

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DevelopmentDB"); // Or "ProductionDB"

// 2. Register the ApplicationDbContext with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Configure Identity services with your custom User class and IdentityRole
builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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
    // options.User.RequireConfirmedEmail = false;
    // options.User.RequireConfirmedPhoneNumber = false;
});

// Configure where unauthenticated/unauthorized users are redirected
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Default login path
    options.AccessDeniedPath = "/Account/AccessDenied"; // Custom Access Denied path
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

// THIS IS THE ONLY PLACE RoleSeeder should be mentioned outside its own file.
// It's a CALL to the seeder, NOT a definition of the seeder class.
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    // Pass IdentityRole to the seeder, as that's what we configured Identity to use for roles.
    await RoleSeeder.SeedRolesAsync(roleManager);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();