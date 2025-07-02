using System.Text.RegularExpressions;

namespace WebAppForeCast.Services
{
	public static class EmailValidator
	{
		private static readonly Regex _emailRegex = new Regex(
			@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
			RegexOptions.Compiled | RegexOptions.IgnoreCase
		);

		public static bool IsValid(string email)
		{
			return !string.IsNullOrEmpty(email) && _emailRegex.IsMatch(email);
		}
	}
}
