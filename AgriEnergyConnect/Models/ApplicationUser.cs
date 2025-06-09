using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Models
{
    // Extend the default IdentityUser if you need custom properties for your users
    public class ApplicationUser : IdentityUser
    {
        // Example: public string FirstName { get; set; }
        // Example: public string LastName { get; set; }
        // public string UserRole { get; set; } // Consider using Identity Roles instead of a custom string role for security
    }
}