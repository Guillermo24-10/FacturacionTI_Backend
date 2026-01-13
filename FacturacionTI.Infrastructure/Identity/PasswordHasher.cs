using FacturacionTI.Application.Interfaces.Security;
using System.Security.Cryptography;

namespace FacturacionTI.Infrastructure.Identity
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GenerateSecureToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
