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

        public string? Role { get; set; } // Add this property
        public string? FarmName { get; set; } // Add this property
        public string? Department { get; set; } // Add this property

        public ICollection<Product> Products { get; set; }
        public ICollection<ForumPost> ForumPosts { get; set; }
        // Add other custom properties as needed
    }
}