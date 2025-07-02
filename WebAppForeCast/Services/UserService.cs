using WebAppForeCast.Data;
using WebAppForeCast.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAppForeCast.Services
{
	public class UserService
	{
		private readonly ApplicationDBContext _context;

		public UserService(ApplicationDBContext context)
		{
			_context = context;
		}

		public async Task<User?> GetUserByUserNameAsync(string username)
		{
			return await _context.Users
				.FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
		}

		public async Task<User?> CreateUserAsync(User user, string password)
		{
			if (await _context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
			{
				return null; // User already exists
			}
			user.PasswordHash = PasswordHasher.HashPassword(password);
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return user;
		}

		public async Task<bool> UpdateUserAsync(int userId, User updatedUser, string newPassword = null)
		{
			var user = await _context.Users.FindAsync(userId);
			if (user == null || !user.IsActive) return false;

			user.Email = updatedUser.Email;
			if (!string.IsNullOrEmpty(newPassword)) { user.PasswordHash = PasswordHasher.HashPassword(newPassword); }

			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteUserAsync(int userId)
		{
			var user = await _context.Users.FindAsync(userId);
			if (user == null || !user.IsActive) return false;
			user.IsActive = false; // Soft delete
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
