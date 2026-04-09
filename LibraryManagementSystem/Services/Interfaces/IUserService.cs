using LibraryManagementSystem.Dtos.UserDto;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        // Task<User?> RegisterUser(CreateUserDto dto);
        Task<UserLoginResponseDto?> LoginUser(GetUserDto dto);
        Task<int> GetUsersCount();
        Task<string> RegisterUser(CreateUserDto dto);
        Task<User> VerifyUser(UserVerifyOtpDto dto);
    }
}
