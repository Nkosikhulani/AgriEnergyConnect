using Microsoft.AspNetCore.Identity; // Must be at the top
using System.Threading.Tasks;      // Must be at the top

namespace AgriEnergyConnect.Data.Seeds
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Ensure "Farmer" role exists
            if (!await roleManager.RoleExistsAsync("Farmer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Farmer"));
            }

            // Ensure "Employee" role exists
            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            // Optional: Ensure "Admin" role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }
    }
}