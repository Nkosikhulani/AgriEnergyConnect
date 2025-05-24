// These 'using' statements MUST be at the very top of the file
using System.ComponentModel.DataAnnotations;
using AgriEnergyConnect.Models; // Ensure this points to where your 'User' model is located

namespace AgriEnergyConnect.ViewModels // This namespace must wrap your class
{
    public class ProfileViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; } // Make nullable

        public string? Role { get; set; } // Make nullable

        public string? FirstName { get; set; } // Make nullable
        public string? LastName { get; set; } // Make nullable
        public string? FarmName { get; set; } // Make nullable
        public string? Department { get; set; } // Make nullable
        public string? Location { get; set; } // Make nullable
        public string? EmployeeId { get; set; } // Make nullable

        public ProfileViewModel() { } // Parameterless constructor
    // ... (rest of the code)


        public ProfileViewModel(User user)
        {
            Email = user.Email;
            Role = user.Role;
            FirstName = user.FirstName;
            LastName = user.LastName;
            FarmName = user.FarmName;
            Department = user.Department;
            Location = user.Location;
            EmployeeId = user.EmployeeId;
        }
    }
}