﻿namespace WebAppForeCast.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public bool IsActive { get; set; } = true;
		// Roles: Admin, User, Guest, etc.
		public string Role { get; set; } = "	User"; // Default role is 'User'
	}
}
