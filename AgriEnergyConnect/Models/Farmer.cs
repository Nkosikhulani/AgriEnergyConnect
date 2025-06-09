using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models
{
    public class Farmer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Location { get; set; }

        public DateTime DateJoined { get; set; } = DateTime.Now;

        public string? UserId { get; set; } // Already made nullable
        public User? User { get; set; } // Already made nullable (Navigation property)
    }
}
