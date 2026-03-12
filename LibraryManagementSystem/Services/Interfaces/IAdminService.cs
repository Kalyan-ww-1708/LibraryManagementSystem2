using LibraryManagementSystem.Dtos.AdminDto;
using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Services.Interfaces
{
    public interface IAdminService
    {
        Task<Admin?> RegisterAdmin(CreateAdminDto dto);
        Task<LoginResponseDto?> LoginAdmin(LoginDto dto);
    }
}
