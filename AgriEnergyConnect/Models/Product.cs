using System.ComponentModel.DataAnnotations;
using AgriEnergyConnect.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
    public string? UserId { get; set; } // Foreign key to User
    public User? User { get; set; }     // Navigation property to User
    // Add other properties as needed
}