using StudentManagementApplication.Interfaces;
using System.Security.Cryptography;

namespace StudentManagementApplication.Middleware;

public class PasswordHasher : IPasswordHasher
{

    private readonly int SaltSize = 64;
    private readonly int HashSize = 256;
    private readonly int Iterations = 350000;
    private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
    
    public async Task<string> HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return string.Concat(Convert.ToHexString(hash), "-", Convert.ToHexString(salt));
    }

    public async Task<bool> VerifyPassword(string password, string HashedPassword)
    {
        string[] PasswordParts = HashedPassword.Split('-');
        byte[] hash = Convert.FromHexString(PasswordParts[0]);
        byte[] salt = Convert.FromHexString(PasswordParts[1]);

        byte[] HashPassword = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, HashPassword);


    }
}
