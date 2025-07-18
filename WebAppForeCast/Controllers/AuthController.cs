﻿using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppForeCast;
using WebAppForeCast.Services;
using WebAppForeCast.Models;
using System.Text.RegularExpressions;

namespace WebAppForeCast.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly UserService _userService;
		private readonly Logerservice _logerservice;
		//private static readonly string _storedPassword = _passwordHasher.HashPassword("admin123");

		public AuthController(IConfiguration configuration, UserService userService, Logerservice logerservice)
		{
			_configuration = configuration;
			_userService = userService;
			_logerservice = logerservice;
			_logerservice = logerservice;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
			{
				await _logerservice.LogAsync("WARNING", "Intento de registro con campos vacíos.");
				return BadRequest(new { message = "El nombre de usuario y la contraseña son obligatorios" });
			}

			if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
			{
				await _logerservice.LogAsync("WARNING", "Correo inválido durante el registro.");
				return BadRequest(new { message = "El correo electrónico no tiene un formato válido" });
			}

			if (!Regex.IsMatch(request.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
			{
				await _logerservice.LogAsync("WARNING", "Contraseña no cumple requisitos mínimos de seguridad.");
				return BadRequest(new
				{
					message = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial"
				});
			}

			var user = new User
			{
				Username = request.Username,
				Email = request.Email,
				Role = request.Role,
			};
			var createdUser = await _userService.CreateUserAsync(user, request.Password);
			if (createdUser == null)
			{
				await _logerservice.LogAsync("WARNING", $"Intento de registro fallido: usuario '{request.Username}' ya existe.");
				return Conflict(new { message = "El usuario ya existe" });
			}

			await _logerservice.LogAsync("INFORMATION", $"Usuario registrado exitosamente: {request.Username}.");
			return Ok(new { message = "Usuario registrado exitosamente" });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
			{
				await _logerservice.LogAsync("WARNING", "Intento de login con campos vacíos.");
				return BadRequest(new { message = "El nombre de usuario y la contraseña son obligatorios" });
			}

			var user = await _userService.GetUserByUserNameAsync(request.Username);

			if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
			{
				await _logerservice.LogAsync("WARNING", $"Login fallido para usuario: {request.Username}.");
				return Unauthorized(new { message = "Credenciales inválidas" });
			}

			await _logerservice.LogAsync("INFORMATION", $"Login exitoso para usuario: {request.Username}.");
			var token = GenerateJwtToken(user.Username, user.Role);
			return Ok(new { token });
		}

		private string GenerateJwtToken(string username, string role)
		{
			var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
			var tokenHandler = new JwtSecurityTokenHandler();

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name, username),
					new Claim(ClaimTypes.Role, role)
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public class LoginRequest
		{
			public string Username { get; set; }
			public string Password { get; set; }
		}

		public class RegisterRequest : LoginRequest
		{
			public string Email { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
			public string Role { get; set; } = "User";
		}
	}
}