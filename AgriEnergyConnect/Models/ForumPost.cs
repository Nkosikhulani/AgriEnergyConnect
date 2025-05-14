using AgriEnergyConnect.Models;
using System;

public class ForumPost
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserId { get; set; } // Foreign key to User
    public User User { get; set; }     // Navigation property to User
    // Add other properties as needed
}