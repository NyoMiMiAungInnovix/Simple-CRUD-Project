using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Simple_CRUD_Project.Abstractions;
using System.Security.Cryptography;

namespace Simple_CRUD_Project.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SALT_SIZE = 128 / 8;
        private const int KEY_SIZE = 256 / 8;
        private const int ITERATIONS = 10000;
        private static readonly HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char DELIMITER = ';';

        public string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SALT_SIZE); //byte
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, ITERATIONS, hashAlgorithmName, KEY_SIZE); //byte

            return string.Join(DELIMITER, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool VerifyHashPassword(string passwordHash, string inputPassword)
        {
            var elements = passwordHash.Split(DELIMITER); //string
            var salt = Convert.FromBase64String(elements[0]); //byte
            var hash = Convert.FromBase64String(elements[1]); //byte

            var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, ITERATIONS, hashAlgorithmName, KEY_SIZE); //byte

            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }
    }
}
