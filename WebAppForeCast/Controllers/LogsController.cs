using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppForeCast.Data;

namespace WebAppForeCast.Controllers
{
	[Authorize(Roles = "Admin")]
	[ApiController]
	[Route("[controller]")]
	public class LogsController : ControllerBase
	{
		private readonly ApplicationDBContext _context;

		public LogsController(ApplicationDBContext context)
		{
			_context = context;
		}

		// Últimas 50 entradas
		[HttpGet("last50")]
		public async Task<IActionResult> GetLast50Logs()
		{
			var logs = await _context.LogEntries
				.OrderByDescending(l => l.Timestamp)
				.Take(50)
				.ToListAsync();

			return Ok(logs);
		}

		// Conteo por nivel
		[HttpGet("count")]
		public async Task<IActionResult> GetCountByLevel([FromQuery] string level)
		{
			if (string.IsNullOrWhiteSpace(level))
				return BadRequest("El parámetro 'level' es requerido.");

			var count = await _context.LogEntries
				.CountAsync(l => l.Level.ToUpper() == level.ToUpper());

			return Ok(new { Level = level.ToUpper(), Count = count });
		}
	}
}
