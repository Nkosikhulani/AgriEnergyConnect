using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Farmer-specific fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FarmName { get; set; }
        public string Location { get; set; }
        public bool FarmerTerms { get; set; } // For the checkbox

        // Employee-specific fields
        public string EmployeeId { get; set; }
        public string Department { get; set; }
        public bool EmployeeTerms { get; set; } // For the checkbox

        // You might still need a general "Role" property if you're assigning roles upon registration
        public string Role { get; set; }
    }
}