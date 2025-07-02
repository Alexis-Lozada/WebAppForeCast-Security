using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppForeCast.Models;
using WebAppForeCast.Services;

namespace WebAppForeCast.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly UserService _userService;

		public UserController(UserService userService)
		{
			_userService = userService;
		}

		// GET /User/{username}
		[HttpGet("{username}")]
		public async Task<IActionResult> GetUserByUsername(string username)
		{
			var user = await _userService.GetUserByUserNameAsync(username);
			if (user == null)
				return NotFound(new { message = "Usuario no encontrado" });

			return Ok(user);
		}

		// PUT /User/update/{username}
		[HttpPut("update/{username}")]
		public async Task<IActionResult> UpdateUser(string username, [FromBody] UpdateUserRequest request)
		{
			var user = await _userService.GetUserByUserNameAsync(username);
			if (user == null)
				return NotFound(new { message = "Usuario no encontrado" });

			var updated = await _userService.UpdateUserAsync(user.Id, new User
			{
				Email = request.Email
			}, request.NewPassword);

			if (!updated)
				return BadRequest(new { message = "No se pudo actualizar el usuario" });

			return Ok(new { message = "Usuario actualizado correctamente" });
		}

		// DELETE /User/delete/{username}
		[HttpDelete("delete/{username}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteUser(string username)
		{
			var user = await _userService.GetUserByUserNameAsync(username);
			if (user == null)
				return NotFound(new { message = "Usuario no encontrado" });

			var deleted = await _userService.DeleteUserAsync(user.Id);
			if (!deleted)
				return BadRequest(new { message = "No se pudo eliminar el usuario" });

			return Ok(new { message = "Usuario eliminado (soft delete)" });
		}

		public class UpdateUserRequest
		{
			public string Email { get; set; } = string.Empty;
			public string? NewPassword { get; set; }
		}
	}
}
