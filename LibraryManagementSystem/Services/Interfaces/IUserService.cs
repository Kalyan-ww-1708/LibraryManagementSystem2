using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> CreateUser(CreateUserDto dto);
        Task<User?> GetUserDetails(GetUserDto dto);
    }
}
