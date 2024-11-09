namespace StudentManagementApplication.Interfaces;

public interface IPasswordHasher
{
    Task<string> HashPassword(string password);
    Task<bool> VerifyPassword(string password, string HashedPassword);
}
