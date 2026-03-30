using System.Globalization;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IPasswordService
    {

        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedpassword);
    }
}
