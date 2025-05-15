using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AgriEnergyConnect.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Products = new HashSet<Product>();
            ForumPosts = new HashSet<ForumPost>();
        }

        public string? Role { get; set; }
        public string? FarmName { get; set; }
        public string? Department { get; set; }

        // Add these new properties
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Location { get; set; }
        public string? EmployeeId { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<ForumPost> ForumPosts { get; set; }
        // Add other custom properties as needed
    }
}