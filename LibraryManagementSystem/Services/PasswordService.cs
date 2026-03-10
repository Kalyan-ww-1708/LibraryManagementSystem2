using LibraryManagementSystem.Services.Interfaces;

using BCrypt.Net;
namespace LibraryManagementSystem.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordService _passwordService;
        public PasswordService(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }
  
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
