using System.Security.Cryptography;

namespace WebAppForeCast.Services
{
	public class PasswordHasher
	{
		private const int saltSize = 16; // Size of the salt in bytes
		private const int hashSize = 32; // Size of the hash in bytes
		private const int iterations = 10000; // Number of iterations for the PBKDF2 algorithm

		public static string HashPassword(string password)
		{
			using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
			{
				byte[]   salt = new byte[saltSize];
				rng.GetBytes(salt);
				using (var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
				{
					byte[] hash = pbkdf2.GetBytes(hashSize);
					byte[] hashBytes = new byte[salt.Length + hash.Length];
					Array.Copy(salt, 0, hashBytes, 0, salt.Length);
					Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);
					return Convert.ToBase64String(hashBytes);
				}
			}
		}

		public static bool VerifyPassword(string password, string hashedPassword)
		{
			byte[] bytes = Convert.FromBase64String(hashedPassword);
			byte[] salt = new byte[saltSize];

			Array.Copy(bytes, 0, salt, 0, saltSize);
			using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
			{
				byte[] hash = pbkdf2.GetBytes(hashSize);
				for (int i = 0; i < hash.Length; i++)
				{
					if (bytes[i + saltSize] != hash[i])
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}