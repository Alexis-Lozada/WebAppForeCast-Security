using System.Text.RegularExpressions;

namespace WebAppForeCast.Services
{
	public static class PasswordValidator
	{
		private static readonly Regex _passwordRegex = new Regex(
			@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
			RegexOptions.Compiled
		);

		public static bool IsValid(string password)
		{
			return !string.IsNullOrEmpty(password) && _passwordRegex.IsMatch(password);
		}
	}
}
