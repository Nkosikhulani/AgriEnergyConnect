using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)] // Added a length requirement for better security
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Core user details, assuming these are always required
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        // Farmer-specific fields - make them nullable as they are not always present (e.g., for Employee registration)
        public string? FarmName { get; set; } // Make nullable
        public string? Location { get; set; } // Make nullable

        // Terms checkboxes: If they MUST be checked, make nullable bool? and add Range validation
        [Display(Name = "I agree to the farmer terms and conditions.")]
        public bool FarmerTerms { get; set; } // If you want to require this to be true, see example below

        // Employee-specific fields - make them nullable as they are not always present (e.g., for Farmer registration)
        public string? EmployeeId { get; set; } // Make nullable
        public string? Department { get; set; } // Make nullable

        [Display(Name = "I agree to the employee terms and conditions.")]
        public bool EmployeeTerms { get; set; } // If you want to require this to be true, see example below

        // Role is almost certainly required, so make it explicitly required
        [Required(ErrorMessage = "Please select your role.")]
        public string Role { get; set; }
    }
}