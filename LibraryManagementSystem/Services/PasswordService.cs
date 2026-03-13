using LibraryManagementSystem.Services.Interfaces;

using BCrypt.Net;
namespace LibraryManagementSystem.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedpassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedpassword);
        }
    }
}
