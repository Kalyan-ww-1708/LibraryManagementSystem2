using LibraryManagementSystem.Model;
using LibraryManagementSystem.Model.UserDto;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> CreateUser(CreateUserDto dto);
        Task<User?> GetUserDetails(GetUserDto dto);
    }
}
