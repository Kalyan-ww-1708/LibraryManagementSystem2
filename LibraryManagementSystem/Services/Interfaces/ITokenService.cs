using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Admin admin);
    }
}
