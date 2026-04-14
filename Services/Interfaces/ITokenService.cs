using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAdminToken(Admin admin);
        string GenerateUserToken(User user);
    }
}
