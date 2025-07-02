namespace WebAppForeCast.Models
{
	public class LogEntry
	{
		public int Id { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		public string Level { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public string? Exception { get; set; }
		public string? Username { get; set; }
		public string? Action { get; set; }
		public string? Controller { get; set; }
	}
}
