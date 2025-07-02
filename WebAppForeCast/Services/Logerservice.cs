using WebAppForeCast.Data;
using WebAppForeCast.Models;

namespace WebAppForeCast.Services
{
	public class Logerservice
	{
		private readonly ApplicationDBContext _context;
		private readonly ILogger<Logerservice> _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public Logerservice(
			ApplicationDBContext context, 
			ILogger<Logerservice> logger, 
			IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task LogAsync(string level, string message, Exception? exception = null)
		{
			var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
			var controller = _httpContextAccessor.HttpContext?.Request.RouteValues["controller"]?.ToString();
			var action = _httpContextAccessor.HttpContext?.Request.RouteValues["action"]?.ToString();

			var logEntry = new LogEntry
			{
				Timestamp = DateTime.UtcNow,
				Level = level,
				Message = message,
				Exception = exception?.ToString(),
				Username = username,
				Controller = controller,
				Action = action
			};

			_context.LogEntries.Add(logEntry);
			await _context.SaveChangesAsync();

			// También logear usando el sistema de logging integrado
			LogToProvider(level, message, exception);
		}

		private void LogToProvider(string level, string message, Exception? exception)
		{
			switch (level.ToUpper())
			{
				case "ERROR":
					_logger.LogError(exception, message);
					break;
				case "WARNING":
					_logger.LogWarning(message);
					break;
				case "INFORMATION":
					_logger.LogInformation(message);
					break;
				default:
					_logger.LogDebug(message);
					break;
			}
		}
	}
}
